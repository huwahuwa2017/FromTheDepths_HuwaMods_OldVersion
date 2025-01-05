using BrilliantSkies.Core.Drawing;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Layouts.DropDowns;
using BrilliantSkies.Ui.Tips;
using ES2_PolygonControl;
using HarmonyLib;
using HuwaTech;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedMimicUi
{
    public class TriangleGeneratorSuperScreen : SuperScreen<int>
    {
        private static GameObject canvasObject;

        private static RectTransform RectTransformA = CreateTextObject("A");

        private static RectTransform RectTransformB = CreateTextObject("B");

        private static RectTransform RectTransformC = CreateTextObject("C");

        private static Vector3[] Coordinates = new Vector3[]
            {
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

        private static void CreateCanvasObject()
        {
            if (canvasObject == null)
            {
                canvasObject = new GameObject("CanvasObject");

                Canvas canvas = canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

        private static RectTransform CreateTextObject(string t)
        {
            CreateCanvasObject();

            GameObject textObject = new GameObject("TextObject");
            textObject.transform.SetParent(canvasObject.transform);

            Text text = textObject.AddComponent<Text>();
            text.text = t;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.color = new Color(0f, 1f, 1f);
            text.fontSize = 24;

            RectTransform rectTransform = textObject.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(50f, 50f);

            return rectTransform;
        }

        public static void CanvasSetActive(bool b)
        {
            canvasObject.SetActive(b);
        }



        private List<PolygonData> polygonDataList = new List<PolygonData>();// PolygonGeneration.GenerateTrianglePolygonData(new int[] { 0, 1, 2 }, Coordinates);

        public override Content Name
        {
            get
            {
                return new Content("Triangle generation", new ToolTip("Generate a triangle from the three coordinates."), "TriangleConstructorSuperScreen");
            }
        }

        public TriangleGeneratorSuperScreen(ConsoleWindow window) : base(window, default)
        {
        }

        public override void Build()
        {
            HuwaUi<int> hUi = new HuwaUi<int>(default);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(1, 11);
            screenSegment_0.SpaceAbove = 10f;
            screenSegment_0.SqueezeTable = false;

            SubjectiveFloatClampedWithBarFromMiddle<int>[] subjectiveFloats = new SubjectiveFloatClampedWithBarFromMiddle<int>[9];

            subjectiveFloats[0] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[0].x), "CoordinateA.x", (int m, float f) => ChangeCoordinate(0, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateA"));
            subjectiveFloats[1] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[0].y), "CoordinateA.y", (int m, float f) => ChangeCoordinate(0, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateA"));
            subjectiveFloats[2] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[0].z), "CoordinateA.z", (int m, float f) => ChangeCoordinate(0, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateA"));

            subjectiveFloats[3] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[1].x), "CoordinateB.x", (int m, float f) => ChangeCoordinate(1, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateB"));
            subjectiveFloats[4] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[1].y), "CoordinateB.y", (int m, float f) => ChangeCoordinate(1, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateB"));
            subjectiveFloats[5] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[1].z), "CoordinateB.z", (int m, float f) => ChangeCoordinate(1, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateB"));

            subjectiveFloats[6] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[2].x), "CoordinateC.x", (int m, float f) => ChangeCoordinate(2, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateC"));
            subjectiveFloats[7] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[2].y), "CoordinateC.y", (int m, float f) => ChangeCoordinate(2, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateC"));
            subjectiveFloats[8] = SubjectiveFloatClampedWithBarFromMiddle<int>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((int I) => Coordinates[2].z), "CoordinateC.z", (int m, float f) => ChangeCoordinate(2, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateC"));

            Array.ForEach(subjectiveFloats,
                (SubjectiveFloatClampedWithBarFromMiddle<int> I) =>
                {
                    I.TextBox.ForceRounding = false;
                    ClassExpansion<ABar, ABar_AddMember>.Access(Traverse.Create(I).Field("Bar").GetValue<ABar>()).FastWidthOptimization = true;
                });

            screenSegment_0.AddInterpretter(subjectiveFloats[0]);
            screenSegment_0.AddInterpretter(subjectiveFloats[1]);
            screenSegment_0.AddInterpretter(subjectiveFloats[2]);

            screenSegment_0.AddInterpretter(new Blank(10f));

            screenSegment_0.AddInterpretter(subjectiveFloats[3]);
            screenSegment_0.AddInterpretter(subjectiveFloats[4]);
            screenSegment_0.AddInterpretter(subjectiveFloats[5]);

            screenSegment_0.AddInterpretter(new Blank(10f));

            screenSegment_0.AddInterpretter(subjectiveFloats[6]);
            screenSegment_0.AddInterpretter(subjectiveFloats[7]);
            screenSegment_0.AddInterpretter(subjectiveFloats[8]);



            ScreenSegmentStandardHorizontal screenSegment_1 = CreateStandardHorizontalSegment();
            screenSegment_1.SpaceAbove = 10f;

            DropDownMenuAlt<StructureBlockType> dropDownMenuAlt = new DropDownMenuAlt<StructureBlockType>();

            dropDownMenuAlt.SetItems(new DropDownMenuAltItem<StructureBlockType>[]
            {
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Wood",
                    ToolTip = "Create a triangle with 'Wood down slope (1m)'",
                    ObjectForAction = StructureBlockType.Wood
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Stone",
                    ToolTip = "Create a triangle with 'Stone down slope (1m)'",
                    ObjectForAction = StructureBlockType.Stone
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Metal",
                    ToolTip = "Create a triangle with 'Metal down slope (1m)'",
                    ObjectForAction = StructureBlockType.Metal
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Alloy",
                    ToolTip = "Create a triangle with 'Alloy down slope (1m)'",
                    ObjectForAction = StructureBlockType.Alloy
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Glass",
                    ToolTip = "Create a triangle with 'Glass down slope (1m)'",
                    ObjectForAction = StructureBlockType.Glass
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Lead",
                    ToolTip = "Create a triangle with 'Lead down slope (1m)'",
                    ObjectForAction = StructureBlockType.Lead
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Heavy armour",
                    ToolTip = "Create a triangle with 'Heavy armour down slope (1m)'",
                    ObjectForAction = StructureBlockType.HeavyArmour
                },
                new DropDownMenuAltItem<StructureBlockType>
                {
                    Name = "Rubber",
                    ToolTip = "Create a triangle with 'Rubber down slope (1m)'",
                    ObjectForAction = StructureBlockType.Rubber
                }
            });

            screenSegment_1.AddInterpretter(new DropDown<int, StructureBlockType>(default, dropDownMenuAlt, (i, I) => PolygonGeneration.SBType.Equals(I), (i, I) => PolygonGeneration.SBType = I));
            screenSegment_1.AddInterpretter(SimpleFloatInput<int>.Quick(default, M.m<int>(I => PolygonGeneration.FaceThickness), new ToolTip("Set the thickness of the triangle to be generated"), (i, f) => PolygonGeneration.FaceThickness = Mathf.Clamp(f, 0.05f, 10f), M.m<int>(I => "Thickness ")));
            screenSegment_1.AddInterpretter(hUi.AddButton(() => (PolygonGeneration.NormalReversal) ? "Embed to the right" : "Embed to the left", () => "Change which side to retract", I => PolygonGeneration.NormalReversal = !PolygonGeneration.NormalReversal));



            ScreenSegmentStandardHorizontal screenSegment_2 = CreateStandardHorizontalSegment();
            screenSegment_2.SetConditionalDisplay(() => polygonDataList.Count == 2);
            screenSegment_2.SpaceAbove = 10f;
            screenSegment_2.SpaceBelow = 20f;

            screenSegment_2.AddInterpretter(hUi.AddButton("Generate a forward triangle", "Generate a triangle", () => TriangleGeneration()));
            screenSegment_2.AddInterpretter(hUi.AddButton("Generate a backward triangle", "Generate a triangle", () => TriangleGeneration(true)));

            ScreenSegmentStandardHorizontal screenSegment_3 = CreateStandardHorizontalSegment();
            screenSegment_3.SetConditionalDisplay(() => polygonDataList.Count == 1);
            screenSegment_3.SpaceAbove = 10f;
            screenSegment_3.SpaceBelow = 20f;

            screenSegment_3.AddInterpretter(hUi.AddButton("Generate a triangle", "Generate a triangle", () => TriangleGeneration()));
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            GetWorldPositionAndWorldRotation(out Vector3 GWP, out Quaternion GWR);

            Vector3 vector_A = GWP + GWR * Coordinates[0];
            Vector3 vector_B = GWP + GWR * Coordinates[1];
            Vector3 vector_C = GWP + GWR * Coordinates[2];

            VectorLines.i.Current.Line(vector_A, vector_B, Color.cyan, 2f, false);
            VectorLines.i.Current.Line(vector_B, vector_C, Color.cyan, 2f, false);
            VectorLines.i.Current.Line(vector_C, vector_A, Color.cyan, 2f, false);

            RectTransformA.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_A);
            RectTransformB.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_B);
            RectTransformC.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_C);

            if (polygonDataList.Count == 0)
            {
                PolygonDataControl.TriangleClassify(polygonDataList, new int[][] { new int[] { 0 }, new int[] { 1 }, new int[] { 2 } }, Coordinates.ToList(), null);
            }
        }

        public override void OnDeactivateGui()
        {
            base.OnDeactivateGui();

            CanvasSetActive(false);
        }

        public virtual bool GetCommonData(out MimicAndDecoration_CommonData MAD_CD)
        {
            MAD_CD = default;
            return false;
        }

        public virtual void GetWorldPositionAndWorldRotation(out Vector3 worldPosition, out Quaternion worldRotation)
        {
            worldPosition = default;
            worldRotation = default;
        }

        private void ChangeCoordinate(int i, string s, float f)
        {
            switch (s)
            {
                case "x":
                    Coordinates[i].x = f;
                    break;
                case "y":
                    Coordinates[i].y = f;
                    break;
                case "z":
                    Coordinates[i].z = f;
                    break;
            }

            polygonDataList.Clear();
            PolygonDataControl.TriangleClassify(polygonDataList, new int[][] { new int[] { 0 }, new int[] { 1 }, new int[] { 2 } }, Coordinates.ToList(), null);
        }

        private void TriangleGeneration(bool flip = false)
        {
            if (GetCommonData(out MimicAndDecoration_CommonData data))
            {
                if (polygonDataList.Count == 2)
                {
                    PolygonGeneration.Generate(data, polygonDataList[(flip) ? 1 : 0]);
                }
                else
                {
                    PolygonGeneration.Generate(data, polygonDataList[0]);
                }
            }
        }
    }
}
