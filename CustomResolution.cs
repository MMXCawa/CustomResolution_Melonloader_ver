using MelonLoader;
using HarmonyLib;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

[assembly: MelonInfo(typeof(CustomResolution.Core), "CustomResolution", "0.0.2", "MMXCawa")]
[assembly: MelonGame("7th Beat Games", "A Dance of Fire and Ice")]

namespace CustomResolution
{
    public enum FullScreenModeType
    {
        Windowed,
        Borderless,
        ExclusiveStretch,
        ExclusiveAspect
    }

    public class Core : MelonMod
    {
        public static Core Instance { get; private set; }

        private bool showWindow;
        private Rect windowRect;
        private Vector2 scroll;

        private int selectedRes;
        private int selectedMode = (int)FullScreenModeType.Borderless;

        private string customWidth = "";
        private string customHeight = "";
        private string errorText = "";

        private readonly string[] modeNames =
        {
            "窗口模式",
            "无边框窗口化",
            "强制全屏（拉伸）",
            "强制全屏（等比例）"
        };

        private List<ResolutionOption> resolutions = new();
        private Resolution native;

        private MelonPreferences_Entry<int> prefWidth;
        private MelonPreferences_Entry<int> prefHeight;
        private MelonPreferences_Entry<int> prefMode;
        private MelonPreferences_Entry<float> prefScale;

        public override void OnInitializeMelon()
        {
            Instance = this;
            native = Screen.currentResolution;

            var cat = MelonPreferences.CreateCategory("CustomResolution");
            prefWidth = cat.CreateEntry("Width", Mathf.Min(1920, native.width));
            prefHeight = cat.CreateEntry("Height", Mathf.Min(1080, native.height));
            prefMode = cat.CreateEntry("FullScreenMode", selectedMode);
            prefScale = cat.CreateEntry("UIScale", 1f);

            SetupDPI();
            LoadResolutions();
            ClampWindow();

            selectedMode = prefMode.Value;
            selectedRes = resolutions.FindIndex(r =>
                r.Width == prefWidth.Value && r.Height == prefHeight.Value);

            if (selectedRes < 0) selectedRes = 0;

            ApplySaved();

            new HarmonyLib.Harmony("com.mmx.customresolution").PatchAll();
            LoggerInstance.Msg("CustomResolution v0.0.2 by MMXCawa\nPress F1 to open menu.");
        }

        public override void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                showWindow = !showWindow;
        }

        public override void OnGUI()
        {
            if (!showWindow) return;

            GUI.matrix = Matrix4x4.Scale(Vector3.one * prefScale.Value);
            windowRect = GUILayout.Window(
                9999, windowRect, Draw,
                "CustomResolution v0.0.1",
                GUILayout.Width(340),
                GUILayout.Height(460)
            );
        }

        private void Draw(int id)
        {
            scroll = GUILayout.BeginScrollView(scroll);

            GUILayout.Label("选择分辨率：");
            selectedRes = GUILayout.SelectionGrid(
                selectedRes,
                resolutions.Select(r => r.ToString()).ToArray(),
                1
            );

            GUILayout.Space(10);
            GUILayout.Label("自定义分辨率（直接应用）：");
            GUILayout.BeginHorizontal();
            customWidth = GUILayout.TextField(customWidth, GUILayout.Width(80));
            GUILayout.Label("×", GUILayout.Width(20));
            customHeight = GUILayout.TextField(customHeight, GUILayout.Width(80));
            GUILayout.EndHorizontal();

            if (GUILayout.Button("应用自定义分辨率"))
                TryApplyCustom();

            if (!string.IsNullOrEmpty(errorText))
                GUILayout.Label($"<color=red>{errorText}</color>");

            GUILayout.Space(10);
            GUILayout.Label($"当前模式：{modeNames[selectedMode]}");
            if (GUILayout.Button("切换全屏模式"))
                selectedMode = (selectedMode + 1) % 4;

            GUILayout.Space(10);
            GUILayout.Label($"UI 缩放：{prefScale.Value:F1}x");
            prefScale.Value = GUILayout.HorizontalSlider(prefScale.Value, 0.5f, 2f);

            GUILayout.Space(10);
            if (GUILayout.Button("应用选中分辨率"))
            {
                ApplySelected();
                MelonPreferences.Save();
            }

            GUILayout.EndScrollView();
            GUI.DragWindow();
        }

        private void TryApplyCustom()
        {
            errorText = "";

            if (!int.TryParse(customWidth, out int w) ||
                !int.TryParse(customHeight, out int h))
            {
                errorText = "请输入有效数字";
                return;
            }

            if (w <= 0 || h <= 0 || w > 19200 || h > 10800)
            {
                errorText = "超出允许范围";
                return;
            }

            // ✅ 直接应用，不进列表
            ApplyDirect(w, h);
        }

        private void ApplyDirect(int w, int h)
        {
            prefWidth.Value = w;
            prefHeight.Value = h;
            prefMode.Value = selectedMode;

            DoApply(w, h);
            LoggerInstance.Msg($"直接应用自定义分辨率：{w}×{h}");
        }

        private void ApplySelected()
        {
            var r = resolutions[selectedRes];
            prefWidth.Value = r.Width;
            prefHeight.Value = r.Height;
            prefMode.Value = selectedMode;

            DoApply(r.Width, r.Height);
        }

        private void ApplySaved()
        {
            DoApply(prefWidth.Value, prefHeight.Value);
        }

        private void DoApply(int w, int h)
        {
            int tw = w, th = h;
            var mode = FullScreenMode.FullScreenWindow;

            switch ((FullScreenModeType)selectedMode)
            {
                case FullScreenModeType.Windowed:
                    mode = FullScreenMode.Windowed;
                    break;
                case FullScreenModeType.Borderless:
                    mode = FullScreenMode.FullScreenWindow;
                    break;
                case FullScreenModeType.ExclusiveStretch:
                    mode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case FullScreenModeType.ExclusiveAspect:
                    mode = FullScreenMode.ExclusiveFullScreen;
                    tw = native.width;
                    th = native.height;
                    break;
            }

            Screen.SetResolution(tw, th, mode);
            ClampWindow();
        }

        private void LoadResolutions()
        {
            resolutions.Clear();

            resolutions.Add(new(native.width, native.height));

            foreach (var r in new[]
            {
                new ResolutionOption(3840,2160),
                new (2560,1440),
                new (1920,1080),
                new (1600,900),
                new (1366,768),
                new (1280,720)
            })
                if (!resolutions.Any(x => x.Width == r.Width && x.Height == r.Height))
                    resolutions.Add(r);
        }

        private void ClampWindow()
        {
            float sw = native.width, sh = native.height;
            windowRect.width = Mathf.Min(340, sw * 0.9f);
            windowRect.height = Mathf.Min(460, sh * 0.9f);
            windowRect.x = Mathf.Clamp(windowRect.x, 0, sw - windowRect.width);
            windowRect.y = Mathf.Clamp(windowRect.y, 0, sh - windowRect.height);
        }

        private void SetupDPI()
        {
            float dpi = Screen.dpi;
            prefScale.Value = dpi > 96 ? Mathf.Clamp(dpi / 96f, 0.5f, 2f) : 1f;
        }

        [HarmonyPatch(typeof(Screen), nameof(Screen.fullScreenMode), MethodType.Setter)]
        public class PatchFS
        {
            static bool Prefix() => false;
        }
    }

    public class ResolutionOption
    {
        public int Width { get; }
        public int Height { get; }

        public ResolutionOption(int w, int h)
        {
            Width = w;
            Height = h;
        }

        public override string ToString() => $"{Width} × {Height}";
    }
}
