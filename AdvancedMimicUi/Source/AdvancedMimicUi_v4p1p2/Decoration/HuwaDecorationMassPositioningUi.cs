using BrilliantSkies.Core.Drawing;
using BrilliantSkies.Core.Help;
using BrilliantSkies.Core.Serialisation.Parameters.Prototypes;
using BrilliantSkies.Core.Types;
using BrilliantSkies.Ftd.Constructs.Modules.All.Decorations;
using BrilliantSkies.Ui.Consoles;
using EndlessShapes2.Polygon;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdvancedMimicUi
{
    public class HuwaDecorationMassPositioningUi : ConsoleUi<DecorationsSubject>
    {
        private struct TempData
        {
            public Decoration _decoration;

            public Vector3 _positioning;

            public Vector3 _orientation;

            public TempData(Decoration deco)
            {
                _decoration = deco;
                _positioning = deco.Positioning.Us;
                _orientation = deco.Orientation.Us;
            }
        }

        private ConsoleWindow _consoleWindow_0;

        private ConsoleWindow _consoleWindow_1;

        private List<TempData> _tempDataList;

        private AllConstruct _targetAllConstruct;

        private MimicAndDecorationCommonData _mainMAD_Data;

        private Decoration _mainSelectDecoration;

        public MimicAndDecorationCommonData SacrificeMAD_Data { get; set; }

        public Decoration SacrificeSelectDecoration { get; set; }

        public bool UndoReservation { get; set; } = true;

        public HuwaDecorationMassPositioningUi(DecorationsSubject subject) : base(subject)
        {
            _tempDataList = _focus.Decorations.Select(I => new TempData(I)).ToList();
            _targetAllConstruct = Traverse.Create(_focus.ConstructDecorations).Property("_construct").GetValue<AllConstruct>();

            _mainMAD_Data = new MimicAndDecorationCommonData();
            SacrificeMAD_Data = new MimicAndDecorationCommonData();
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            _consoleWindow_0 = NewWindow(0, "Decorations editor", new ScaledRectangle(10f, 10f, 600f, 0f));
            _consoleWindow_0.DisplayTextPrompt = false;
            _consoleWindow_0.SetMultipleTabs(new HuwaDecorationMassPositioningTab(_consoleWindow_0, _focus, this));

            _consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 230f));
            _consoleWindow_1.DisplayTextPrompt = false;
            _consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            _consoleWindow_1.SetMultipleTabs(new HuwaDecorationMassPositioningControlTab(_consoleWindow_1, _focus, this), new HuwaCalculatorTab(_consoleWindow_1), new HuwaDecorationSettingsTab(_consoleWindow_1, _focus));

            return _consoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            Vector3 originPosition = _tempDataList.FirstOrDefault(I => I._decoration == SacrificeSelectDecoration)._positioning;
            Vector3 positioning = SacrificeMAD_Data.Positioning;
            Vector3 orientation = SacrificeMAD_Data.Orientation;

            bool flag_0 = _mainMAD_Data.Positioning != positioning;
            bool flag_1 = _mainMAD_Data.Orientation != orientation;
            bool flag_2 = _mainSelectDecoration != SacrificeSelectDecoration;

            if (flag_0 || flag_1 || flag_2)
            {
                if (flag_0) _mainMAD_Data.Positioning = positioning;
                if (flag_1) _mainMAD_Data.Orientation = orientation;
                if (flag_2) _mainSelectDecoration = SacrificeSelectDecoration;

                if (_mainSelectDecoration == null)
                {
                    foreach (TempData td in _tempDataList)
                    {
                        td._decoration.Positioning.Us = Quaternion.Euler(orientation) * td._positioning + positioning;
                        td._decoration.Orientation.Us = (Quaternion.Euler(orientation) * Quaternion.Euler(td._orientation)).eulerAngles;
                    }
                }
                else
                {
                    List<Vector3> relativePositionList = _tempDataList.Select(I => I._positioning - originPosition).ToList();

                    for (int index = 0; index < _tempDataList.Count; index++)
                    {
                        TempData td = _tempDataList[index];
                        Vector3 relativePosition = relativePositionList[index];

                        td._decoration.Positioning.Us = Quaternion.Euler(orientation) * relativePosition + originPosition + positioning;
                        td._decoration.Orientation.Us = (Quaternion.Euler(orientation) * Quaternion.Euler(td._orientation)).eulerAngles;
                    }
                }
            }

            Vector3i tetherPoint = Traverse.Create(_focus).Field("_point").GetValue<Vector3i>();
            Vector3 localPosition = tetherPoint + originPosition + positioning;
            Vector3 up = Quaternion.Euler(0f, Time.time * 360f % 360f, 0f) * Vector3.forward;
            VectorLines.i.Current.Circle(_targetAllConstruct.SafeLocalToGlobal(localPosition), 0.15f, Color.magenta, up, 2f, 4);
        }

        public override void OnDeactivateGui()
        {
            base.OnDeactivateGui();

            if (UndoReservation)
            {
                foreach (TempData td in _tempDataList)
                {
                    td._decoration.Positioning.Us = td._positioning;
                    td._decoration.Orientation.Us = td._orientation;
                }
            }
            else
            {
                foreach (TempData td in _tempDataList)
                {
                    VarVector3 temp_0 = td._decoration.Positioning;
                    VarVector3 temp_1 = td._decoration.Orientation;
                    Vector3 temp_2 = temp_0.Us;
                    Vector3 temp_3 = temp_1.Us;
                    temp_0.Us = new Vector3(Rounding.R4(temp_2.x), Rounding.R4(temp_2.y), Rounding.R4(temp_2.z));
                    temp_1.Us = new Vector3(Rounding.R4(temp_3.x), Rounding.R4(temp_3.y), Rounding.R4(temp_3.z));
                }
            }
        }

        public void PositionReset()
        {
            if (_mainSelectDecoration != null)
            {
                Vector3 temp_0 = _tempDataList.FirstOrDefault(I => I._decoration == _mainSelectDecoration)._positioning;
                SacrificeMAD_Data.Positioning = -temp_0;
            }
        }

        public void RotationReset()
        {
            if (_mainSelectDecoration != null)
            {
                Vector3 temp_0 = _tempDataList.FirstOrDefault(I => I._decoration == _mainSelectDecoration)._orientation;
                SacrificeMAD_Data.Orientation = Quaternion.Inverse(Quaternion.Euler(temp_0)).eulerAngles;
            }
        }

        public DecorationsSubject GetDecorationsSubject()
        {
            return _focus;
        }

        public bool GetDecoration(out Decoration deco)
        {
            if (_consoleWindow_0 != null)
            {
                ConsoleUiScreen screen = _consoleWindow_0.Screen;

                if (screen is HuwaDecorationEditorTab)
                {
                    deco = Traverse.Create(screen as HuwaDecorationEditorTab).Field("_focus").GetValue<Decoration>();
                    return true;
                }
            }

            deco = null;
            return false;
        }
    }
}
