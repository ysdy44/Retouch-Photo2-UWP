// Core:              ★★★★★
// Referenced:   ★★★★★
// Difficult:         ★★★★★
// Only:              ★★★★★
// Complete:      ★★★★★

namespace Retouch_Photo2.Historys
{
    /// <summary>
    /// Type of <see cref="IHistory"/>.
    /// </summary>
    public enum HistoryType
    {
        None,


        //$DrawPage
        LayersSetupTransformAdd_Move,
        LayersSetupTransformMultiplies_Transform,
        
        LayeragesArrange_LayersArrange,
        LayeragesArrange_RemoveLayers,


        //Tool
        LayeragesArrange_AddLayer,
        LayeragesArrange_AddLayer_ConvertToCurves,

        LayersTransformAdd_Move,
        LayersTransformMultiplies_Transform,

        LayersProperty_SetTransform_IsCrop,
        LayersProperty_SetTransform_CropTransformer,
        LayersProperty_SetTransform_ResetTransformer,
        LayersProperty_SetTransform_FitTransformer,
        LayersProperty_SetTransform_ClearTransformer,

        LayersProperty_SetStyle,
        LayersProperty_SetStyle_Fill,
        LayersProperty_SetStyle_Fill_Extend,
        LayersProperty_SetStyle_Fill_Type,
        LayersProperty_SetStyle_Stroke,
        LayersProperty_SetStyle_Stroke_Extend,
        LayersProperty_SetStyle_Stroke_Type,

        LayersProperty_SetStyle_Transparency,
        LayersProperty_SetStyle_Transparency_Type,

        LayersProperty_Set_MoveNodes,
        LayersProperty_Set_MoveNode_ControlPoint,
        LayersProperty_Set_MoveNode_IsChecked,
        LayersProperty_Set_AddNode,
        LayersProperty_Set_RemoveNodes,
        LayersProperty_Set_InsertNodes,
        LayersProperty_Set_SharpNodes,
        LayersProperty_Set_SmoothNodes,

        LayersProperty_SetPhotocopier,

        LayersProperty_Set_PatternGridLayer_GridType,
        LayersProperty_Set_PatternGridLayer_HorizontalStep,
        LayersProperty_Set_PatternGridLayer_VerticalStep,
        LayersProperty_Set_PatternDiagonalLayer_Offset,
        LayersProperty_Set_PatternDiagonalLayer_HorizontalStep,
        LayersProperty_Set_PatternSpottedLayer_Radius,
        LayersProperty_Set_PatternSpottedLayer_Step,

        LayersProperty_Set_GeometryRoundRectLayer_Corner,
        LayersProperty_Set_GeometryTriangleLayer_Center,
        LayersProperty_Set_GeometryDiamondLayer_Mid,
        LayersProperty_Set_GeometryDiamondLayer_HoleRadius,
        LayersProperty_Set_GeometryPentagonLayer_Points,
        LayersProperty_Set_GeometryStarLayer_Points,
        LayersProperty_Set_GeometryStarLayer_InnerRadius,
        LayersProperty_Set_GeometryCogLayer_Count,
        LayersProperty_Set_GeometryCogLayer_InnerRadius,
        LayersProperty_Set_GeometryCogLayer_Tooth,
        LayersProperty_Set_GeometryCogLayer_Notch,
        LayersProperty_Set_GeometryDountLayer_HoleRadius,
        LayersProperty_Set_GeometryPieLayer_SweepAngle,
        LayersProperty_Set_GeometryCookieLayer_InnerRadius,
        LayersProperty_Set_GeometryCookieLayer_SweepAngle,
        LayersProperty_Set_GeometryArrowLayer_LeftTail,
        LayersProperty_Set_GeometryArrowLayer_RightTail,
        LayersProperty_Set_GeometryArrowLayer_Value,
        LayersProperty_Set_GeometryHeartLayer_Spread,

         
        //Menu
        LayersProperty_SetName,
        LayersProperty_SetBlendMode,
        LayersProperty_SetOpacity,
        LayersProperty_SetOpacity_000,
        LayersProperty_SetOpacity_025,
        LayersProperty_SetOpacity_050,
        LayersProperty_SetOpacity_075,
        LayersProperty_SetOpacity_100,
        LayersProperty_SetVisibility,
        LayersProperty_SetIsSelected,
        LayersProperty_SetTagType,


        //Edit
        LayeragesArrange_CutLayers,
        LayeragesArrange_DuplicateLayer,
        LayeragesArrange_DuplicateLayers,
        LayeragesArrange_PasteLayers,
        LayeragesArrange_ClearLayers,

        LayeragesArrange_GroupLayers,
        LayeragesArrange_UngroupLayers,
        LayeragesArrange_ReleaseLayers,

        LayeragesArrange_AddLayer_Combine,

        LayeragesArrange_AddLayer_ExpandStroke,
        LayeragesArrange_AddLayers_ExpandStroke,


        //Adjustment
        LayersProperty_ResetAdjustment_Exposure,
        LayersProperty_SetAdjustment_Exposure_Exposure,

        LayersProperty_ResetAdjustment_Brightness,
        LayersProperty_SetAdjustment_Brightness_WhiteLight,
        LayersProperty_SetAdjustment_Brightness_WhiteDark,
        LayersProperty_SetAdjustment_Brightness_BlackLight,
        LayersProperty_SetAdjustment_Brightness_BlackDark,

        LayersProperty_ResetAdjustment_Saturation,
        LayersProperty_SetAdjustment_Saturation_Saturation,

        LayersProperty_ResetAdjustment_HueRotation,
        LayersProperty_SetAdjustment_HueRotation_Angle,

        LayersProperty_ResetAdjustment_Contrast,
        LayersProperty_ResetAdjustment_Contrast_Contrast,

        LayersProperty_ResetAdjustment_Temperature,
        LayersProperty_SetAdjustment_Temperature_Temperature,
        LayersProperty_SetAdjustment_Temperature_Tint,

        LayersProperty_ResetAdjustment_HighlightsAndShadows,
        LayersProperty_SetAdjustment_HighlightsAndShadows_Shadows,
        LayersProperty_SetAdjustment_HighlightsAndShadows_Highlights,
        LayersProperty_SetAdjustment_HighlightsAndShadows_Clarity,
        LayersProperty_SetAdjustment_HighlightsAndShadows_MaskBlurAmount,

        LayersProperty_ResetAdjustment_GammaTransfer,
        LayersProperty_SetAdjustment_GammaTransfer_AlphaDisable,
        LayersProperty_SetAdjustment_GammaTransfer_AlphaOffset,
        LayersProperty_SetAdjustment_GammaTransfer_AlphaExponent,
        LayersProperty_SetAdjustment_GammaTransfer_AlphaAmplitude,
        LayersProperty_SetAdjustment_GammaTransfer_RedDisable,
        LayersProperty_SetAdjustment_GammaTransfer_RedOffset,
        LayersProperty_SetAdjustment_GammaTransfer_RedExponent,
        LayersProperty_SetAdjustment_GammaTransfer_RedAmplitude,
        LayersProperty_SetAdjustment_GammaTransfer_GreenDisable,
        LayersProperty_SetAdjustment_GammaTransfer_GreenOffset,
        LayersProperty_SetAdjustment_GammaTransfer_GreenExponent,
        LayersProperty_SetAdjustment_GammaTransfer_GreenAmplitude,
        LayersProperty_SetAdjustment_GammaTransfer_BlueDisable,
        LayersProperty_SetAdjustment_GammaTransfer_BlueOffset,
        LayersProperty_SetAdjustment_GammaTransfer_BlueExponent,
        LayersProperty_SetAdjustment_GammaTransfer_BlueAmplitude,

        LayersProperty_ResetAdjustment_Vignette,
        LayersProperty_SetAdjustment_Vignette_Amount,
        LayersProperty_SetAdjustment_Vignette_Curve,
        LayersProperty_SetAdjustment_Vignette_Color,


        //Effect
        LayersProperty_ResetEffect_GaussianBlur,
        LayersProperty_SwitchEffect_GaussianBlur,
        LayersProperty_SetEffect_GaussianBlur_Amount,
        LayersProperty_SetEffect_GaussianBlur_BoderMode,

        LayersProperty_ResetEffect_DirectionalBlur,
        LayersProperty_SwitchEffect_DirectionalBlur,
        LayersProperty_SetEffect_DirectionalBlur_Radius,
        LayersProperty_SetEffect_DirectionalBlur_Angle,
        LayersProperty_SetEffect_DirectionalBlur_BoderMode,

        LayersProperty_ResetEffect_Sharpen,
        LayersProperty_SwitchEffect_Sharpen,
        LayersProperty_SetEffect_Sharpen_Amount,

        LayersProperty_ResetEffect_OuterShadow,
        LayersProperty_SwitchEffect_OuterShadow,
        LayersProperty_SetEffect_OuterShadow_Radius,
        LayersProperty_SetEffect_OuterShadow_Opacity,
        LayersProperty_SetEffect_OuterShadow_Offset,
        LayersProperty_SetEffect_OuterShadow_Angle,
        LayersProperty_SetEffect_OuterShadow_Color,

        LayersProperty_ResetEffect_Edge,
        LayersProperty_SwitchEffect_Edge,
        LayersProperty_SetEffect_Edge_Amount,
        LayersProperty_SetEffect_Edge_Radius,

        LayersProperty_ResetEffect_Mmorphology,
        LayersProperty_SwitchEffect_Mmorphology,
        LayersProperty_SetEffect_Mmorphology_Size,

        LayersProperty_ResetEffect_Emboss,
        LayersProperty_SwitchEffect_Emboss,
        LayersProperty_SetEffect_Emboss_Radius,
        LayersProperty_SetEffect_Emboss_Angle,

        LayersProperty_ResetEffect_Straighten,
        LayersProperty_SwitchEffect_Straighten,
        LayersProperty_SetEffect_Straighten_Angle,


        //Text
        LayersProperty_SetFontText,
        LayersProperty_SetFontSize,
        LayersProperty_SetFontFamily,

        LayersProperty_SetHorizontalAlignment,
        LayersProperty_SetDirection,

        LayersProperty_SetUnderline,
        LayersProperty_SetFontStyle,
        LayersProperty_SetFontWeight,


        //Stroke
        LayersProperty_SetStyle_StrokeStyle_Dash,
        LayersProperty_SetStyle_StrokeWidth,
        LayersProperty_SetStyle_StrokeStyle_Cap,
        LayersProperty_SetStyle_StrokeStyle_Join,
        LayersProperty_SetStyle_StrokeStyle_Offset,

        LayersProperty_SetStyle_IsFollowTransform,
        LayersProperty_SetStyle_IsStrokeBehindFill,
        LayersProperty_SetStyle_IsStrokeWidthFollowScale,


        //Filter
        LayersProperty_SetFilter,
    }
}