using BrilliantSkies.Core;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Elements;
using BrilliantSkies.Ui.Tips;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaSearcher<T> : SubjectiveAbstract<int>
    {
        private static List<T> _history = new List<T>();



        private readonly Func<List<T>> _getList;

        private readonly Func<T, string, bool> _searchFilter;

        private IVS<string, T> _typeToString;

        private IVS<IToolTip, T> _typeToToolTip;

        private Action<T> _fnOnSelect;

        private List<T> _list;

        private List<T> _filteredList;

        private string _preSearchText = string.Empty;

        private int _numberOfDisplay = 200;

        private int _selectPage = 0;

        public HuwaSearcher(Func<string> displayString, Func<IToolTip> tooltip, Func<List<T>> getList, Func<T, string, bool> searchFilter, IVS<string, T> typeToString, IVS<IToolTip, T> typeToToolTip, Action<T> fnOnSelect, params string[] keys) : base(default, M.m<int>(I => displayString()), M.m<int>(I => tooltip()), keys)
        {
            _getList = getList;
            _searchFilter = searchFilter;
            _typeToString = typeToString;
            _typeToToolTip = typeToToolTip;
            _fnOnSelect = fnOnSelect;
        }

        public override string GetKeyString(string key)
        {
            return key;
        }

        protected override bool ProcessInputOnceKeyFound(List<string> wordsInInputExcludingKey, bool apply, ref string outcome)
        {
            return false;
        }

        public override void Draw(SO_BuiltUi styles)
        {
            if (_list == null)
            {
                _list = _getList();
                _filteredList = HuwaSettingsTab.ViewHistory ? _history : new List<T>();
            }

            GUILayout.BeginHorizontal(styles.TextEdit.NameBoxPanel.Style);
            styles.TextEdit.NameBoxLeftHandSide.Layout(DisplayString.GetFromSubject(Subject), ToolTipCode, GetOptions());
            string searchText = styles.TextEdit.TextEnter.TextField(_preSearchText, GetOptions());
            GUILayout.EndHorizontal();

            bool searchTextINOE = string.IsNullOrEmpty(searchText);

            if (searchText != _preSearchText)
            {
                _preSearchText = searchText;

                _selectPage = 0;

                if (searchTextINOE)
                {
                    _filteredList = HuwaSettingsTab.ViewHistory ? _history : new List<T>();
                }
                else
                {
                    //_filteredList = (from t in _list where _searchFilter(t, _searchString) select t).ToList<TType>();
                    _filteredList = _list.Where(t => _searchFilter(t, searchText)).ToList();
                }
            }

            int filteredListCount = _filteredList.Count;

            if (!searchTextINOE)
            {
                GUILayout.Label($"About {filteredListCount} results", styles.Display.DisplayText.Style);
            }

            InputType inputType = 0;
            int startIndex = _selectPage * _numberOfDisplay;
            int endIndex = Math.Min((_selectPage + 1) * _numberOfDisplay, filteredListCount);

            void ChangePage()
            {
                if (filteredListCount > _numberOfDisplay)
                {
                    GUILayout.BeginHorizontal();

                    inputType = styles.Buttons.Button.LayoutButton("<");

                    if (inputType == InputType.LeftClick)
                    {
                        _selectPage = Math.Max(_selectPage - 1, 0);
                    }

                    GUILayout.Label("Page : " + (_selectPage + 1), styles.Display.DisplayText.Style);

                    inputType = styles.Buttons.Button.LayoutButton(">");

                    if (inputType == InputType.LeftClick)
                    {
                        _selectPage = Math.Min(_selectPage + 1, (filteredListCount - 1) / _numberOfDisplay);
                    }

                    GUILayout.EndHorizontal();
                }
            }

            ChangePage();

            GUILayout.Space(10f);

            for (int i = startIndex; i < endIndex; i++)
            {
                T ttype = _filteredList[i];
                string toolTipTex = string.Format("Terp{0}", i);
                inputType = styles.Buttons.Button.LayoutButton(_typeToString.GetFromSubject(ttype), toolTipTex);

                if (GUI.tooltip == toolTipTex)
                {
                    TipDisplayer.Instance.SetTip(_typeToToolTip.GetFromSubject(ttype));
                }

                if (inputType == InputType.LeftClick)
                {
                    _fnOnSelect(ttype);
                    _history = new T[] { ttype }.Concat(_history).Distinct().ToList();
                }
                else if (inputType == InputType.RightClick && searchTextINOE)
                {
                    _history.Remove(ttype);
                    _filteredList = _history;
                }
            }

            GUILayout.Space(10f);

            ChangePage();
        }
    }
}
