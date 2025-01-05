using BrilliantSkies.Core.Drawing;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Numbers;
using BrilliantSkies.Ui.Consoles.Segments;
using BrilliantSkies.Ui.Layouts.DropDowns;
using BrilliantSkies.Ui.Tips;
using EndlessShapes2.Polygon;
using HarmonyLib;
using HuwaTech;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdvancedMimicUi
{
    public class HuwaTriangleGeneratorTab : SuperScreen<int>
    {
        private static readonly DropDownMenuAltItem<StructureBlockType>[] downMenuAltItems =
            new DropDownMenuAltItem<StructureBlockType>[]
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
            };

        private static GameObject _canvasObject;

        private static RectTransform _rectTransformA = CreateTextObject("A");

        private static RectTransform _rectTransformB = CreateTextObject("B");

        private static RectTransform _rectTransformC = CreateTextObject("C");

        private static Vector3[] _coordinates = new Vector3[]
            {
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

        private static void CreateCanvasObject()
        {
            if (_canvasObject == null)
            {
                _canvasObject = new GameObject("CanvasObject");

                Canvas canvas = _canvasObject.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }
        }

        private static RectTransform CreateTextObject(string t)
        {
            CreateCanvasObject();

            GameObject textObject = new GameObject("TextObject");
            textObject.transform.SetParent(_canvasObject.transform);

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
            _canvasObject.SetActive(b);
        }



        private List<PolygonData> _polygonDataList = new List<PolygonData>();

        public override Content Name
        {
            get
            {
                return new Content("Triangle generation", new ToolTip("Generate a triangle from the three coordinates."), "TriangleConstructorSuperScreen");
            }
        }

        public HuwaTriangleGeneratorTab(ConsoleWindow window) : base(window, default)
        {
        }

        public override void Build()
        {
            HuwaUi<byte> hUi = new HuwaUi<byte>(default);

            ScreenSegmentTable screenSegment_0 = CreateTableSegment(1, 11);
            screenSegment_0.SqueezeTable = false;
            screenSegment_0.SpaceAbove = 10f;

            SubjectiveFloatClampedWithBarFromMiddle<byte>[] subjectiveFloats = new SubjectiveFloatClampedWithBarFromMiddle<byte>[9];

            subjectiveFloats[0] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[0].x), "CoordinateA.x", (byte m, float f) => ChangeCoordinate(0, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateA"));
            subjectiveFloats[1] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[0].y), "CoordinateA.y", (byte m, float f) => ChangeCoordinate(0, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateA"));
            subjectiveFloats[2] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[0].z), "CoordinateA.z", (byte m, float f) => ChangeCoordinate(0, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateA"));

            subjectiveFloats[3] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[1].x), "CoordinateB.x", (byte m, float f) => ChangeCoordinate(1, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateB"));
            subjectiveFloats[4] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[1].y), "CoordinateB.y", (byte m, float f) => ChangeCoordinate(1, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateB"));
            subjectiveFloats[5] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[1].z), "CoordinateB.z", (byte m, float f) => ChangeCoordinate(1, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateB"));

            subjectiveFloats[6] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[2].x), "CoordinateC.x", (byte m, float f) => ChangeCoordinate(2, "x", f), new ToolTip("Adjust the left right positioning of the CoordinateC"));
            subjectiveFloats[7] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[2].y), "CoordinateC.y", (byte m, float f) => ChangeCoordinate(2, "y", f), new ToolTip("Adjust the up down positioning of the CoordinateC"));
            subjectiveFloats[8] = SubjectiveFloatClampedWithBarFromMiddle<byte>.Quick(default, -15f, 15f, 0.05f, 0f,
                M.m((byte I) => _coordinates[2].z), "CoordinateC.z", (byte m, float f) => ChangeCoordinate(2, "z", f), new ToolTip("Adjust the forward backward positioning of the CoordinateC"));

            foreach (SubjectiveFloatClampedWithBarFromMiddle<byte> s in subjectiveFloats)
            {
                s.TextBox.ForceRounding = false;
                HuwaUi.ABar_AddMember_Access(s).FastWidthOptimization = true;

                s.ClampInput = false;
            }

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
            dropDownMenuAlt.SetItems(downMenuAltItems);

            SubjectiveToggle<byte> subjectiveToggle = SubjectiveToggle<byte>.Quick(default, "Normal reversal", new ToolTip("Set the direction to shift the face"),
                (byte I, bool b) =>
                {
                    MADCD_PolygonConstruction.NormalReversal = b;
                },
                (byte I) =>
                {
                    return MADCD_PolygonConstruction.NormalReversal;
                });

            screenSegment_1.AddInterpretter(new DropDown<byte, StructureBlockType>(default, dropDownMenuAlt, (i, I) => MADCD_PolygonConstruction.SBType.Equals(I), (i, I) => MADCD_PolygonConstruction.SBType = I));
            screenSegment_1.AddInterpretter(SimpleFloatInput<byte>.Quick(default, M.m<byte>(I => MADCD_PolygonConstruction.FaceThickness), new ToolTip("Set the thickness of the triangle to be generated"), (i, f) => MADCD_PolygonConstruction.FaceThickness = Mathf.Clamp(f, 0.05f, 10f), M.m<byte>(I => "Thickness ")));
            screenSegment_1.AddInterpretter(subjectiveToggle);



            ScreenSegmentStandardHorizontal screenSegment_2 = CreateStandardHorizontalSegment();
            screenSegment_2.SetConditionalDisplay(() => _polygonDataList.Count == 2);
            screenSegment_2.SpaceAbove = 10f;
            screenSegment_2.SpaceBelow = 20f;

            screenSegment_2.AddInterpretter(hUi.AddButton("Generate a forward triangle", "Generate a triangle", () => TriangleGeneration()));
            screenSegment_2.AddInterpretter(hUi.AddButton("Generate a backward triangle", "Generate a triangle", () => TriangleGeneration(true)));

            ScreenSegmentStandardHorizontal screenSegment_3 = CreateStandardHorizontalSegment();
            screenSegment_3.SetConditionalDisplay(() => _polygonDataList.Count == 1);
            screenSegment_3.SpaceAbove = 10f;
            screenSegment_3.SpaceBelow = 20f;

            screenSegment_3.AddInterpretter(hUi.AddButton("Generate a triangle", "Generate a triangle", () => TriangleGeneration()));
        }

        public override void LateUpdateWhenActive()
        {
            base.LateUpdateWhenActive();

            GetWorldPositionAndWorldRotation(out Vector3 wp, out Quaternion wr);

            Vector3 vector_A = wp + wr * _coordinates[0];
            Vector3 vector_B = wp + wr * _coordinates[1];
            Vector3 vector_C = wp + wr * _coordinates[2];

            VectorLines.i.Current.Line(vector_A, vector_B, Color.cyan, 2f, false);
            VectorLines.i.Current.Line(vector_B, vector_C, Color.cyan, 2f, false);
            VectorLines.i.Current.Line(vector_C, vector_A, Color.cyan, 2f, false);

            _rectTransformA.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_A);
            _rectTransformB.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_B);
            _rectTransformC.position = RectTransformUtility.WorldToScreenPoint(Camera.main, vector_C);

            if (_polygonDataList.Count == 0)
            {
                PolygonDataControl.TriangleClassify(_polygonDataList, new int[][] { new int[] { 0 }, new int[] { 1 }, new int[] { 2 } }, _coordinates.ToList(), null);
            }
        }

        public override void OnDeactivateGui()
        {
            base.OnDeactivateGui();

            CanvasSetActive(false);
        }

        public virtual bool GetCommonData(out MimicAndDecorationCommonData madcd)
        {
            madcd = default;
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
                    _coordinates[i].x = f;
                    break;
                case "y":
                    _coordinates[i].y = f;
                    break;
                case "z":
                    _coordinates[i].z = f;
                    break;
            }

            _polygonDataList.Clear();
            PolygonDataControl.TriangleClassify(_polygonDataList, new int[][] { new int[] { 0 }, new int[] { 1 }, new int[] { 2 } }, _coordinates.ToList(), null);
        }

        private void TriangleGeneration(bool flip = false)
        {
            if (GetCommonData(out MimicAndDecorationCommonData data))
            {
                MADCD_PolygonConstruction.ColorSetting = null;

                if (_polygonDataList.Count == 2)
                {
                    MADCD_PolygonConstruction.PolygonConstruction(data, _polygonDataList[(flip) ? 1 : 0]);
                }
                else
                {
                    MADCD_PolygonConstruction.PolygonConstruction(data, _polygonDataList[0]);
                }
            }
        }
    }
}
