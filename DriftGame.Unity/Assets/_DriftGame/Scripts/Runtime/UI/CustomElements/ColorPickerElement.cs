using System;
using UnityEngine;
using UnityEngine.UIElements;

[UnityEngine.Scripting.Preserve]
public class ColorPickerElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ColorPickerElement, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription hueAttr = new UxmlFloatAttributeDescription{ name="hue" , defaultValue=0.5f };
        UxmlFloatAttributeDescription brightnessAttr = new UxmlFloatAttributeDescription{ name="brightness" , defaultValue=1f };
        public override void Init (VisualElement ve, IUxmlAttributes attributes, CreationContext context)
        {
            base.Init(ve, attributes, context);
            var instance = (ColorPickerElement) ve;
            instance.Hue = hueAttr.GetValueFromBag(attributes,context);
            instance.Brightness = brightnessAttr.GetValueFromBag(attributes,context);
        }
    }

    public float Hue { get; set; }
    public float Brightness { get; set; }

    public Color Color => Color.HSVToRGB(1-Hue % 1, 1, Brightness);
    public event Action<Color> OnColorPicked;

    public Gradient CircleGradient { get; set; } = new Gradient{
        mode = GradientMode.Blend ,
        colorKeys = new GradientColorKey[]{
            new GradientColorKey( Color.HSVToRGB(0,1,1) , 0 ) ,
            new GradientColorKey( Color.HSVToRGB(1*1f/6f,1,1) , 1*1f/6f ) ,
            new GradientColorKey( Color.HSVToRGB(2*1f/6f,1,1) , 2*1f/6f ) ,
            new GradientColorKey( Color.HSVToRGB(3*1f/6f,1,1) , 3*1f/6f ) ,
            new GradientColorKey( Color.HSVToRGB(4*1f/6f,1,1) , 4*1f/6f ) ,
            new GradientColorKey( Color.HSVToRGB(5*1f/6f,1,1) , 5*1f/6f ) ,
            new GradientColorKey( Color.HSVToRGB(1,1,1) , 1 ) ,
        }
    };

    public ColorPickerElement ()
    {
        generateVisualContent += OnGenerateVisualContent;
        RegisterCallback<ClickEvent>(OnMouseClicked);
        RegisterCallback<MouseLeaveEvent>(OnDragExited);
    }

    private void OnDragExited(MouseLeaveEvent evt)
    {
        Vector2 dir = evt.localMousePosition - contentRect.center;
        Hue = 0.25f + Mathf.Atan2(-dir.y,dir.x) / Mathf.PI  * -0.5f;
        Rect rect = contentRect;
        float swh = Mathf.Min( rect.width , rect.height );
        Brightness = dir.magnitude / (swh*0.4f);
        MarkDirtyRepaint();
        OnColorPicked?.Invoke(Color);
    }

    private void OnMouseClicked(ClickEvent evt)
    {
        Vector2 dir = (Vector2)evt.localPosition - contentRect.center;
        Hue = 0.25f + Mathf.Atan2(-dir.y,dir.x) / Mathf.PI  * -0.5f;
        Rect rect = contentRect;
        float swh = Mathf.Min( rect.width , rect.height );
        Brightness = dir.magnitude / (swh*0.4f);
        MarkDirtyRepaint();
        OnColorPicked?.Invoke(Color);
    }

    private void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        Rect rect = contentRect;
        float swh = Mathf.Min( rect.width , rect.height );
        
        if (swh < 0.01f)
        {
            return;
        }

        var paint = mgc.painter2D;
        float circleRadius = swh * 0.4f;
        float gradientWidth = swh * 0.05f;

        paint.BeginPath();
        {
            paint.Arc( rect.center , circleRadius-gradientWidth/2 , 0 , 360 );
            paint.fillColor = Color;
            paint.Fill();
        }
        paint.ClosePath();
        
        paint.BeginPath();
        {
            paint.Arc( rect.center , circleRadius , 270-0.001f , -90 , ArcDirection.CounterClockwise );

            paint.lineWidth = gradientWidth + 0.2f;
            paint.strokeColor = Color.black;
            paint.Stroke();

            paint.lineWidth = gradientWidth;
            paint.strokeGradient = CircleGradient;
            paint.Stroke();
        }
        paint.ClosePath();
        paint.BeginPath();
        {
            float hueAngle = -Mathf.PI / 2 + Hue * Mathf.PI * 2;
            paint.Arc( rect.center + Vector2.Scale(new Vector2(circleRadius,circleRadius),new Vector2(Mathf.Cos(hueAngle),Mathf.Sin(hueAngle))) , swh*0.03f , 0 , 360 );
            paint.lineWidth = 0.4f;
            paint.strokeColor = Color.white;
            paint.Stroke();
        }
        paint.ClosePath();
    }
}