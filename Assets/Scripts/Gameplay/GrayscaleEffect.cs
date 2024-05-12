using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GrayscaleEffect : MonoBehaviour {
    
    public Shader m_GrayScaleShader;
    [Range(0,1)]
    public float m_GrayscaleStrength;

    Material m_GrayscaleMat;
    bool m_Fading;

	// Use this for initialization
	void Awake () {
        if (m_GrayScaleShader)
            m_GrayscaleMat = new Material(m_GrayScaleShader);	
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!m_GrayScaleShader)
            return;

        if(!m_GrayscaleMat)
            m_GrayscaleMat = new Material(m_GrayScaleShader);   

        m_GrayscaleMat.SetFloat("_Grayscale", m_GrayscaleStrength);

        Graphics.Blit(source, destination, m_GrayscaleMat);
    }
}
