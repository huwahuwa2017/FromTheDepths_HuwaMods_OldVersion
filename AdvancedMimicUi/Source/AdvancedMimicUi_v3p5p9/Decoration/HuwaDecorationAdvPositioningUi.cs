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
    public class HuwaDecorationAdvPositioningUi : ConsoleUi<DecorationsSubject>
    {
        private struct TempData
        {
            public Decoration decoration;

            public Vector3 positioning;

            public Vector3 orientation;

            public TempData(Decoration deco)
            {
                decoration = deco;
                positioning = deco.Positioning.Us;
                orientation = deco.Orientation.Us;
            }
        }

        private ConsoleWindow consoleWindow_0;

        private ConsoleWindow consoleWindow_1;

        private List<TempData> tempDataList;

        private AllConstruct targetAllConstruct;

        private MimicAndDecorationCommonData mainMAD_Data;

        private Decoration mainSelectDecoration;

        public MimicAndDecorationCommonData SacrificeMAD_Data { get; set; }

        public Decoration SacrificeSelectDecoration { get; set; }

        public bool UndoReservation { get; set; } = true;

        public HuwaDecorationAdvPositioningUi(DecorationsSubject subject) : base(subject)
        {
            tempDataList = _focus.Decorations.Select(I => new TempData(I)).ToList();
            targetAllConstruct = Traverse.Create(_focus.ConstructDecorations).Property("_construct").GetValue<AllConstruct>();

            mainMAD_Data = new MimicAndDecorationCommonData();
            SacrificeMAD_Data = new MimicAndDecorationCommonData();
        }

        protected override ConsoleWindow BuildInterface(string suggestedName = "")
        {
            consoleWindow_0 = NewWindow(0, "Decorations editor", new ScaledRectangle(10f, 10f, 600f, 0f));
            consoleWindow_0.DisplayTextPrompt = false;
            consoleWindow_0.SetMultipleTabs(new HuwaDecorationAdvPositioningTab(consoleWindow_0, _focus, this));

            consoleWindow_1 = NewWindow(2, "Other", new ScaledRectangle(670f, 10f, 600f, 230f));
            consoleWindow_1.DisplayTextPrompt = false;
            consoleWindow_1.MinimumWindowHeight = new PixelSizing(35f, Dimension.Height);
            consoleWindow_1.SetMultipleTabs(new HuwaDecorationAdvPositioningControlTab(consoleWindow_1, _focus, this), new CalculationSuperScreen(consoleWindow_1), new HuwaDecorationSettingsTab(consoleWindow_1, _focus));

            return consoleWindow_0;
        }

        public override void LateUpdateWhenActive()
        {
            Vector3 originPosition = tempDataList.FirstOrDefault(I => I.decoration == SacrificeSelectDecoration).positioning;
            Vector3 positioning = SacrificeMAD_Data.Positioning;
            Vector3 orientation = SacrificeMAD_Data.Orientation;

            bool flag_0 = mainMAD_Data.Positioning != positioning;
            bool flag_1 = mainMAD_Data.Orientation != orientation;
            bool flag_2 = mainSelectDecoration != SacrificeSelectDecoration;

            if (flag_0 || flag_1 || flag_2)
            {
                if (flag_0) mainMAD_Data.Positioning = positioning;
                if (flag_1) mainMAD_Data.Orientation = orientation;
                if (flag_2) mainSelectDecoration = SacrificeSelectDecoration;

                if (mainSelectDecoration == null)
                {
                    foreach (TempData td in tempDataList)
                    {
                        td.decoration.Positioning.Us = Quaternion.Euler(orientation) * td.positioning + positioning;
                        td.decoration.Orientation.Us = (Quaternion.Euler(orientation) * Quaternion.Euler(td.orientation)).eulerAngles;
                    }
                }
                else
                {
                    List<Vector3> relativePositionList = tempDataList.Select(I => I.positioning - originPosition).ToList();

                    for (int index = 0; index < tempDataList.Count; index++)
                    {
                        TempData td = tempDataList[index];
                        Vector3 relativePosition = relativePositionList[index];

                        td.decoration.Positioning.Us = Quaternion.Euler(orientation) * relativePosition + originPosition + positioning;
                        td.decoration.Orientation.Us = (Quaternion.Euler(orientation) * Quaternion.Euler(td.orientation)).eulerAngles;
                    }
                }
            }

            Vector3i tetherPoint = Traverse.Create(_focus).Field("_point").GetValue<Vector3i>();
            Vector3 localPosition = tetherPoint + originPosition + positioning;
            Vector3 up = Quaternion.Euler(0f, Time.time * 360f % 360f, 0f) * Vector3.forward;
            VectorLines.i.Current.Circle(targetAllConstruct.SafeLocalToGlobal(localPosition), 0.15f, Color.magenta, up, 2f, 4);
        }

        public override void OnDeactivateGui()
        {
            base.OnDeactivateGui();

            if (UndoReservation)
            {
                foreach (TempData td in tempDataList)
                {
                    td.decoration.Positioning.Us = td.positioning;
                    td.decoration.Orientation.Us = td.orientation;
                }
            }
            else
            {
                foreach (TempData td in tempDataList)
                {
                    VarVector3 temp_0 = td.decoration.Positioning;
                    VarVector3 temp_1 = td.decoration.Orientation;
                    Vector3 temp_2 = temp_0.Us;
                    Vector3 temp_3 = temp_1.Us;
                    temp_0.Us = new Vector3(Rounding.R4(temp_2.x), Rounding.R4(temp_2.y), Rounding.R4(temp_2.z));
                    temp_1.Us = new Vector3(Rounding.R4(temp_3.x), Rounding.R4(temp_3.y), Rounding.R4(temp_3.z));
                }
            }
        }

        public void PositionReset()
        {
            if (mainSelectDecoration != null)
            {
                Vector3 temp_0 = tempDataList.FirstOrDefault(I => I.decoration == mainSelectDecoration).positioning;
                SacrificeMAD_Data.Positioning = -temp_0;
            }
        }

        public void RotationReset()
        {
            if (mainSelectDecoration != null)
            {
                Vector3 temp_0 = tempDataList.FirstOrDefault(I => I.decoration == mainSelectDecoration).orientation;
                SacrificeMAD_Data.Orientation = Quaternion.Inverse(Quaternion.Euler(temp_0)).eulerAngles;
            }
        }

        public DecorationsSubject GetDecorationsSubject()
        {
            return _focus;
        }

        public bool GetDecoration(out Decoration deco)
        {
            if (consoleWindow_0 != null)
            {
                ConsoleUiScreen screen = consoleWindow_0.Screen;

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
