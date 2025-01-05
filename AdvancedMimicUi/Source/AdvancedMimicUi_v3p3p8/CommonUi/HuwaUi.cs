using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Buttons;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Tips;
using HarmonyLib;
using HuwaTech;
using System;
using System.Reflection;
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
        private ConsoleWindow Window;

        private int UpdateCount;

        private Vector2 PreScale;

        public ResizeWindowHeight(ConsoleWindow window)
        {
            Window = window;
        }

        public void Update()
        {
            if (UpdateCount >= 2)
            {
                return;
            }

            Rect rect = Window.GetRectRequired();
            rect.height -= 15f;

            if (UpdateCount == 0)
            {
                PreScale = new Vector2(rect.width, rect.height);

                Window.ResizeMeToMinSize = true;
            }

            if (UpdateCount == 1)
            {
                object[] args = new object[] { rect.x, rect.y, PreScale.x, PreScale.y };
                Pixel pixel = Activator.CreateInstance(typeof(Pixel), (BindingFlags)36, null, args, null) as Pixel;

                //Window.MinimumWindowWidth = new PixelSizing(rect.width, Dimension.Width);
                Window.MinimumWindowHeight = new PixelSizing(rect.height + 5f, Dimension.Height);
                Window.SetSizeAndPosition(pixel);
            }

            ++UpdateCount;
        }
    }
}
