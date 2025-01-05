using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Tips;
using HarmonyLib;
using HuwaTech;
using System;
using UnityEngine;

namespace AdvancedMimicUi
{
    public static class HuwaUi
    {
        public static ABar_AddMember ABar_AddMember_Access<T>(BarsCommon<T> barsCommon)
        {
            return ClassExpansion<ABar, ABar_AddMember>.Access(Traverse.Create(barsCommon).Field("Bar").GetValue<ABar>());
        }
    }

    public class HuwaUi<T>
    {
        private T _focus;

        public T Focus { get => _focus; set => _focus = value; }

        public HuwaUi(T focus)
        {
            _focus = focus;
        }

        public SubjectiveButton<T> AddButton(Func<T, string> label, Func<T, string> tip, Action<T> action)
        {
            return new SubjectiveButton<T>(_focus, M.m(label), M.m<T>(I => new ToolTip(tip(I))), null, action)
            {
                WrapText = false
            };
        }

        public SubjectiveButton<T> AddButton(Func<string> label, Func<string> tip, Action<T> action)
        {
            return AddButton(I => label(), I => tip(), action);
        }

        public SubjectiveButton<T> AddButton(string label, string tip, Action<T> action)
        {
            return AddButton(I => label, I => tip, action);
        }

        public SubjectiveButton<T> AddButton(string label, string tip, Action action)
        {
            return AddButton(I => label, I => tip, I => action());
        }

        public SubjectiveButton<T> FlipButton(Action<T> action)
        {
            return AddButton("+ -", "Change of sign", action);
        }
    }

    public class ResizeWindowHeight
    {
        private ConsoleWindow _window;

        private int _updateCount;

        private Vector2 _preScale;

        public ResizeWindowHeight(ConsoleWindow window)
        {
            _window = window;
        }

        public void Update()
        {
            if (_updateCount >= 2)
            {
                return;
            }

            Rect rect = _window.GetRectRequired();
            rect.height -= 15f;

            if (_updateCount == 0)
            {
                _preScale = new Vector2(rect.width, rect.height);

                _window.ResizeMeToMinSize = true;
            }

            if (_updateCount == 1)
            {
                //object[] args = new object[] { rect.x, rect.y, PreScale.x, PreScale.y };
                //Pixel pixel = Activator.CreateInstance(typeof(Pixel), (BindingFlags)36, null, args, null) as Pixel;
                Pixel pixel = new Pixel(rect.x, rect.y, _preScale.x, _preScale.y);

                //Window.MinimumWindowWidth = new PixelSizing(rect.width, Dimension.Width);
                _window.MinimumWindowHeight = new PixelSizing(rect.height + 5f, Dimension.Height);
                _window.SetSizeAndPosition(pixel);
            }

            ++_updateCount;
        }
    }
}
