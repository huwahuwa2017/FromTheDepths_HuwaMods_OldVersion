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
        private static List<T> history = new List<T>();



        private readonly Func<List<T>> getList;

        private readonly Func<T, string, bool> searchFilter;

        private IVS<string, T> typeToString;

        private IVS<IToolTip, T> typeToToolTip;

        private Action<T> fnOnSelect;

        private List<T> list;

        private List<T> filteredList;

        private string searchString = "";

        public HuwaSearcher(Func<string> displayString, Func<IToolTip> tooltip, Func<List<T>> getList, Func<T, string, bool> searchFilter, IVS<string, T> typeToString, IVS<IToolTip, T> typeToToolTip, Action<T> fnOnSelect, params string[] keys) : base(default, M.m<int>(I => displayString()), M.m<int>(I => tooltip()), keys)
        {
            this.getList = getList;
            this.searchFilter = searchFilter;
            this.typeToString = typeToString;
            this.typeToToolTip = typeToToolTip;
            this.fnOnSelect = fnOnSelect;
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
            if (list == null)
            {
                list = getList();
                filteredList = SettingsSuperScreen.ViewHistory ? history : new List<T>();
            }

            GUILayout.BeginHorizontal(styles.TextEdit.NameBoxPanel.Style);
            styles.TextEdit.NameBoxLeftHandSide.Layout(DisplayString.GetFromSubject(Subject), ToolTipCode, GetOptions());
            string text = styles.TextEdit.TextEnter.TextField(searchString, GetOptions());
            GUILayout.EndHorizontal();

            if (text != searchString)
            {
                searchString = text;

                if (string.IsNullOrEmpty(searchString))
                {
                    filteredList = SettingsSuperScreen.ViewHistory ? history : new List<T>();
                }
                else
                {
                    //this._filteredList = (from t in this._list where this._searchFilter(t, this._searchString) select t).ToList<TType>();
                    filteredList = list.Where(t => searchFilter(t, searchString)).ToList();
                }
            }

            for (int i = 0; i < filteredList.Count; i++)
            {
                T ttype = filteredList[i];
                string toolTipTex = string.Format("Terp{0}", i);
                InputType inputType = styles.Buttons.Button.LayoutButton(typeToString.GetFromSubject(ttype), toolTipTex);

                if (GUI.tooltip == toolTipTex)
                {
                    TipDisplayer.Instance.SetTip(typeToToolTip.GetFromSubject(ttype));
                }

                if (inputType == InputType.LeftClick)
                {
                    fnOnSelect(ttype);
                    history = new T[] { ttype }.Concat(history).Distinct().ToList();
                }
                else if (inputType == InputType.RightClick && string.IsNullOrEmpty(searchString))
                {
                    history.Remove(ttype);
                    filteredList = history;
                }
            }
        }
    }
}
