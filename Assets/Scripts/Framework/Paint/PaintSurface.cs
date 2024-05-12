using System.Collections;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

[DefaultExecutionOrder(20)]
public class PaintSurface : MonoBehaviour
{
    public static int s_Size = 256;
    private static int s_PixCount;

    // Cache
    private Transform m_Transform;
    private Texture2D m_Texture;
    private MeshRenderer m_Renderer;

    // Buffers
    private float m_PlaneSize;
    private float m_HalfPlaneSize;
    private int[][] m_HashMatrix;
    private int m_OldHash;
    private int m_NewHash;
    private readonly int m_X;
    private readonly int m_Y;
    private Hashtable m_Colors;
    private Color32[] m_TexturePixels;
    private int m_OldPixCount;
    private int m_PixCount;
    private int m_DefaultColorHash;
    private Vector3 m_Position;
    private bool m_HasChanged;

    void Awake()
    {
        s_Size = 256;
        s_PixCount = s_Size * s_Size;

        // Cache
        m_Texture = new Texture2D(s_Size, s_Size, TextureFormat.RGBA32, false, false)
        {
            filterMode = FilterMode.Point
        };

        m_Renderer = GetComponent<MeshRenderer>();
        m_Renderer.material.mainTexture = m_Texture;
        m_Transform = transform;
        m_Position = m_Transform.position;

        // Buffers
        m_HasChanged = false;
        m_PlaneSize = Mathf.Max(m_Renderer.bounds.size.x, m_Renderer.bounds.size.z);
        m_HalfPlaneSize = m_PlaneSize * 0.5f;
        m_Colors = new Hashtable();
        m_OldPixCount = 0;
        m_PixCount = 0;
    }

    public void Init(Color32 _Color)
    {
        int i, j;

        m_DefaultColorHash = GetColorHash(_Color);
        m_HashMatrix = new int[s_Size][];
        m_TexturePixels = new Color32[s_Size * s_Size];

        for (i = 0; i < s_Size; ++i)
        {
            m_HashMatrix[i] = new int[s_Size];
            for (j = 0; j < s_Size; ++j)
            {
                m_TexturePixels[i * s_Size + j] = _Color;
                m_HashMatrix[i][j] = m_DefaultColorHash;
            }
        }

        m_Texture.SetPixels32(m_TexturePixels);
        m_Texture.Apply();
    }

    public void ReplaceColor(Vector3 _Center, float _StartWidth, float _EndWidth, int _OldColorHash, int _NewColorHash, ref Color _NewColor)
    {
        _Center -= m_Position;

        int xTopLeft_1, yTopLeft_1;
        PosToMatrix(_Center.x - _EndWidth, _Center.z - _EndWidth, out xTopLeft_1, out yTopLeft_1);
        xTopLeft_1 = Mathf.Clamp(xTopLeft_1, 0, s_Size);
        yTopLeft_1 = Mathf.Clamp(yTopLeft_1, 0, s_Size);

        int xTopLeft_2, yTopLeft_2;
        PosToMatrix(_Center.x - _StartWidth, _Center.z - _StartWidth, out xTopLeft_2, out yTopLeft_2);
        xTopLeft_2 = Mathf.Clamp(xTopLeft_2, 0, s_Size);
        yTopLeft_2 = Mathf.Clamp(yTopLeft_2, 0, s_Size);

        int xDownRight_1, yDownRight_1;
        PosToMatrix(_Center.x + _StartWidth, _Center.z + _StartWidth, out xDownRight_1, out yDownRight_1);
        xDownRight_1 = Mathf.Clamp(xDownRight_1, 0, s_Size);
        yDownRight_1 = Mathf.Clamp(yDownRight_1, 0, s_Size);

        int xDownRight_2, yDownRight_2;
        PosToMatrix(_Center.x + _EndWidth, _Center.z + _EndWidth, out xDownRight_2, out yDownRight_2);
        xDownRight_2 = Mathf.Clamp(xDownRight_2, 0, s_Size);
        yDownRight_2 = Mathf.Clamp(yDownRight_2, 0, s_Size);

        m_PixCount = 0;
        m_OldPixCount = 0;

        // Top rect
        ReplaceColorInRange(
            Mathf.Min(xTopLeft_1, xDownRight_2),
            Mathf.Max(xTopLeft_1, xDownRight_2),
            Mathf.Min(yTopLeft_1, yTopLeft_2),
            Mathf.Max(yTopLeft_1, yTopLeft_2),
            _OldColorHash,
            _NewColorHash,
            ref _NewColor);

        // Left rect
        ReplaceColorInRange(
            Mathf.Min(xTopLeft_1, xTopLeft_2),
            Mathf.Max(xTopLeft_1, xTopLeft_2),
            Mathf.Min(yTopLeft_1, yDownRight_2),
            Mathf.Max(yTopLeft_1, yDownRight_2),
            _OldColorHash,
            _NewColorHash,
            ref _NewColor);

        // Down rect
        ReplaceColorInRange(
            Mathf.Min(xTopLeft_1, xDownRight_2),
            Mathf.Max(xTopLeft_1, xDownRight_2),
            Mathf.Min(yDownRight_1, yDownRight_2),
            Mathf.Max(yDownRight_1, yDownRight_2),
            _OldColorHash,
            _NewColorHash,
            ref _NewColor);

        // Right rect
        ReplaceColorInRange(
            Mathf.Min(xDownRight_1, xDownRight_2),
            Mathf.Max(xDownRight_1, xDownRight_2),
            Mathf.Min(yTopLeft_1, yDownRight_2),
            Mathf.Max(yTopLeft_1, yDownRight_2),
            _OldColorHash,
            _NewColorHash,
            ref _NewColor);

        // Add new color
        if (m_PixCount > 0)
        {
            if (m_Colors.Contains(m_NewHash))
                m_Colors[m_NewHash] = ((int)m_Colors[m_NewHash]) + m_PixCount;
            else
                m_Colors[m_NewHash] = m_PixCount;
        }

        // Remove old color
        if (m_OldPixCount > 0)
        {
            if (m_Colors.Contains(m_OldHash))
                m_Colors[m_OldHash] = ((int)m_Colors[m_OldHash]) - m_OldPixCount;
            else
                m_Colors[m_OldHash] = 0;
        }
    }

    private void ReplaceColorInRange(int _XStart, int _XEnd, int _YStart, int _YEnd, int _OldColorHash, int _NewColorHash, ref Color _NewColor)
    {
        int x, y;
        for (x = _XStart; x < _XEnd; ++x)
        {
            for (y = _YStart; y < _YEnd; ++y)
            {
                if (x < 0 || x > s_Size - 1 ||
                    y < 0 || y > s_Size - 1)
                    continue;

                if (m_HashMatrix[x][y] == _OldColorHash)
                    UpdatePixel(x, y, _NewColorHash, ref _NewColor);
            }
        }
    }

    private bool IsInIgnoreRadius(int x, int y, int ignoreX, int ignoreY, int ignoreSize)
    {
        return !(x < ignoreX || x > ignoreX + ignoreSize ||
            y < ignoreY || y > ignoreY + ignoreSize);
    }

    public void PosToMatrix(float _XPos, float _ZPos, out int _XMat, out int _YMat)
    {
        float xPercent = 1.0f - (_XPos + m_HalfPlaneSize) / m_PlaneSize;
        float zPercent = 1.0f - (_ZPos + m_HalfPlaneSize) / m_PlaneSize;
        _XMat = (int)(xPercent * ((float)s_Size));
        _YMat = (int)(zPercent * ((float)s_Size));
    }

    private void MatrixToPos(int _XMat, int _YMat, out float _XPos, out float _ZPos)
    {
        float xPercent = 1.0f - ((float)_XMat) / ((float)s_Size);
        float yPercent = 1.0f - ((float)_YMat) / ((float)s_Size);
        _XPos = m_PlaneSize * xPercent - m_HalfPlaneSize;
        _ZPos = m_PlaneSize * yPercent - m_HalfPlaneSize;
    }

    public void AddCircle(Vector3 _Position, float _Radius, int _ColorHash, ref Color _Color)
    {
        // Check if the circle overlaps with this surface
        if (_Position.x + _Radius < m_Position.x - m_HalfPlaneSize ||
            _Position.x - _Radius > m_Position.x + m_HalfPlaneSize ||
            _Position.z + _Radius < m_Position.z - m_HalfPlaneSize ||
            _Position.z - _Radius > m_Position.z + m_HalfPlaneSize)
            return;

        _Position -= m_Position;

        float radPercent = _Radius / m_PlaneSize;

        int radiusPix = (int)System.Math.Ceiling(radPercent * ((float)s_Size));
        int sqrRadiusPix = radiusPix * radiusPix;

        int xPix, yPix;
        PosToMatrix(_Position.x, _Position.z, out xPix, out yPix);

        int xStart = Mathf.Clamp(xPix - radiusPix, 0, s_Size);
        int xEnd = Mathf.Clamp(xPix + radiusPix, 0, s_Size);
        int yStart = Mathf.Clamp(yPix - radiusPix, 0, s_Size);
        int yEnd = Mathf.Clamp(yPix + radiusPix, 0, s_Size);

        m_PixCount = 0;
        m_OldPixCount = 0;

        for (int x = xStart; x < xEnd; ++x)
        {
            for (int y = yStart; y < yEnd; ++y)
            {
                if (x < 0 || x > s_Size - 1 ||
                    y < 0 || y > s_Size - 1)
                    continue;

                int oldHash = m_HashMatrix[x][y];
                if (oldHash == _ColorHash)
                    continue;

                int xSub = x - xPix;
                int ySub = y - yPix;
                int sqrMag = xSub * xSub + ySub * ySub;

                if (sqrMag < sqrRadiusPix)
                    UpdatePixel(x, y, _ColorHash, ref _Color);
            }
        }

        // Add new color
        if (m_PixCount > 0)
        {
            if (m_Colors.Contains(_ColorHash))
                m_Colors[_ColorHash] = ((int)m_Colors[_ColorHash]) + m_PixCount;
            else
                m_Colors[_ColorHash] = m_PixCount;
        }
    }

    public float GetColorPercent(int _ColorHash)
    {
        if (m_Colors.ContainsKey(_ColorHash))
        {
            int pixCount = (int)m_Colors[_ColorHash];
            return (((float)pixCount) / ((float)s_PixCount));
        }
        else
            return 0.0f;
    }

    private void UpdatePixel(int _X, int _Y, int _ColorHash, ref Color _Color)
    {
        int currentHash = m_HashMatrix[_X][_Y];
        m_NewHash = _ColorHash;

        if (currentHash != m_DefaultColorHash)
        {
            if (m_Colors.Contains(currentHash))
                m_Colors[currentHash] = ((int)m_Colors[currentHash]) - 1;
            else
                m_Colors[currentHash] = 0;
        }

        m_OldHash = currentHash;
        m_HashMatrix[_X][_Y] = _ColorHash;
        m_TexturePixels[_Y * s_Size + _X] = _Color;
        m_HasChanged = true;
        m_PixCount++;
    }

    public static int GetColorHash(Color _Color)
    {
        return ((int)(10.0f * _Color.r) +
            ((int)(10.0f * _Color.g) * 100) +
            ((int)(10.0f * _Color.b) * 10000));
    }

    void Update()
    {
        if (!m_HasChanged)
            return;

        m_HasChanged = false;
        m_Texture.SetPixels32(m_TexturePixels);
        m_Texture.Apply();
    }

    public float GetWidth()
    {
        return m_Transform.localScale.x;
    }

    public float GetHeight()
    {
        return m_Transform.localScale.z;
    }
}
