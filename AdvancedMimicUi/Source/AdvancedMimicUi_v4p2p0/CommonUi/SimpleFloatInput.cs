using BrilliantSkies.Core;
using BrilliantSkies.Core.StringGarbage;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Displayer;
using BrilliantSkies.Ui.Tips;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class SimpleFloatInput<I> : SubjectiveAbstract<I>
    {
        public ISizing _prescribedWidthForLabel = null;

        public bool _useTextArea = false;

        private bool _isFocused = false;

        private float _preNum = 0f;

        private string _myText = null;

        private readonly Action<I, float> _actionToDo = null;

        private readonly Func<I, string, string> _effectOfAction = null;

        private readonly Func<string, string> _stringCleaner = null;

        private readonly Func<I, string, string> _stringChecker = null;

        private readonly IVS<float, I> _fnGetStringCurrently = null;

        private static readonly StringComposerMultiple<string> _composer = new StringComposerMultiple<string>();

        public SimpleFloatInput(I subject, IVS<float, I> fnGetStringCurrently, IVS<IToolTip, I> toolTip, Action<I, float> actionToDo, Func<I, string, string> effectOfAction, Func<string, string> stringCleaner, Func<I, string, string> stringChecker, IVS<string, I> displayString = null, params string[] keys) : base(subject, displayString, toolTip, keys)
        {
            _actionToDo = actionToDo;
            _effectOfAction = effectOfAction;
            _stringCleaner = stringCleaner;
            _stringChecker = stringChecker;
            _fnGetStringCurrently = fnGetStringCurrently;
        }

        public static SimpleFloatInput<I> Quick(I subject, IVS<float, I> getString, ToolTip tip, Action<I, float> changeAction, IVS<string, I> displayString = null)
        {
            return new SimpleFloatInput<I>(subject, getString, M.m<I>(tip), changeAction, null, (string s) => s, (I I, string s) => null, displayString, Array.Empty<string>());
        }

        public override string GetKeyString(string key)
        {
            return string.Format("{0}<size=10>[your text here]</size>", key);
        }

        public override void Draw(SO_BuiltUi styles)
        {
            float currentNum = _fnGetStringCurrently.GetFromSubject(Subject);

            if (_myText == null || (_preNum != currentNum && float.TryParse(_myText, out _)))
            {
                _preNum = currentNum;
                _myText = currentNum.ToString();
            }

            bool simpleUi = DisplayString == null;

            if (!simpleUi)
            {
                GUILayout.BeginHorizontal(styles.TextEdit.NameBoxPanel.Style, Array.Empty<GUILayoutOption>());
                string displayString = base.GetDisplayString();
                bool flag = !string.IsNullOrEmpty(displayString);
                if (flag)
                {
                    bool flag2 = _prescribedWidthForLabel != null;
                    if (flag2)
                    {
                        styles.TextEdit.NameBoxLeftHandSide.Layout(displayString, base.ToolTipCode, new GUILayoutOption[]
                        {
                        GUILayout.Width(_prescribedWidthForLabel.Pixels)
                        });
                    }
                    else
                    {
                        styles.TextEdit.NameBoxLeftHandSide.Layout(displayString, base.ToolTipCode, Array.Empty<GUILayoutOption>());
                    }
                }
            }

            styles.TextEdit.TextEnter.WrapOnceOff = base.WrapText;

            if (!simpleUi)
            {
                base.PreFocus();
            }

            string controlName = _composer.Make("HuwaMimicUi ", ToolTipCode);
            GUI.SetNextControlName(controlName);

            if (_useTextArea)
            {
                _myText = styles.TextEdit.TextEnter.TextArea(_myText, GetOptions());
            }
            else
            {
                _myText = styles.TextEdit.TextEnter.TextField(_myText, GetOptions());
            }

            if (!simpleUi)
            {
                base.PostFocus();

                GUILayout.EndHorizontal();
            }



            float num = StringToFloat(_myText);

            void ApplyFloat()
            {
                _myText = _stringCleaner(_myText);

                if (_stringChecker(base.Subject, _myText) == null)
                {
                    _actionToDo(base.Subject, num);
                }

                _preNum = num;
            };

            if (controlName == GUI.GetNameOfFocusedControl())
            {
                ApplyFloat();

                if (GuiDisplayer.GetSingleton().EventWrapper.CheckKeyPressInTextBox(KeyCode.Return, controlName))
                {
                    _myText = num.ToString();
                }

                _isFocused = true;
            }
            else
            {
                if (_isFocused)
                {
                    ApplyFloat();

                    _myText = num.ToString();
                }

                _isFocused = false;
            }

            if (GUI.tooltip == base.ToolTipCode)
            {
                TipDisplayer.Instance.SetTip(GetToolTip);
            }
        }

        protected override bool ProcessAParticularKey(string key, string input, bool apply, ref string outcome)
        {
            input = input.TrimStart(new char[]
            {
                ' '
            });
            int num = input.IndexOf(' ');
            bool flag = num == -1;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                string a = input.Substring(0, num);
                string item = input.Substring(num + 1);
                bool flag2 = string.Equals(a, key, StringComparison.CurrentCultureIgnoreCase);
                if (flag2)
                {
                    bool flag3 = ProcessInputOnceKeyFound(new List<string>
                    {
                        item
                    }, apply, ref outcome);
                    if (flag3)
                    {
                        return true;
                    }
                }
                result = false;
            }
            return result;
        }

        protected override bool ProcessInputOnceKeyFound(List<string> wordsInInputExcludingKey, bool apply, ref string outcome)
        {
            bool flag = wordsInInputExcludingKey.Count == 0;
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                string text = wordsInInputExcludingKey[0];
                text = _stringCleaner(text);
                bool flag2 = _stringChecker(base.Subject, text) == null;
                if (flag2)
                {
                    outcome = _effectOfAction(base.Subject, text);
                    if (apply)
                    {
                        _actionToDo(base.Subject, StringToFloat(text));
                    }
                    result = true;
                }
                else
                {
                    outcome = _stringChecker(base.Subject, text);
                    result = false;
                }
            }
            return result;
        }

        private float StringToFloat(string s)
        {
            if (float.TryParse(s, out float num))
            {
                return num;
            }

            return 0f;
        }
    }
}
