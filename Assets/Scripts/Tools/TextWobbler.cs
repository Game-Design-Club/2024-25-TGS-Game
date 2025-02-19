using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextWobbler : MonoBehaviour
{
    [Header("Wobble Settings")]
    public float wobbleAmplitude = 5f;

    public float wobbleFrequency = 2f;
    
    public float wobbleSpacing = 1f;

    private TextMeshProUGUI _tmpText;
    private TMP_TextInfo _textInfo;

    void Awake()
    {
        TryGetComponent(out _tmpText);
    }

    void Update()
    {
        _tmpText.ForceMeshUpdate();
        _textInfo = _tmpText.textInfo;

        for (int i = 0; i < _textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = _textInfo.characterInfo[i];

            if (!charInfo.isVisible)
                continue;

            int vertexIndex = charInfo.vertexIndex;
            int materialIndex = charInfo.materialReferenceIndex;
            Vector3[] vertices = _textInfo.meshInfo[materialIndex].vertices;

            float offsetY = Mathf.Sin(Time.time * wobbleFrequency + i * wobbleSpacing) * wobbleAmplitude;
            Vector3 offset = new Vector3(0, offsetY, 0);

            vertices[vertexIndex + 0] += offset;
            vertices[vertexIndex + 1] += offset;
            vertices[vertexIndex + 2] += offset;
            vertices[vertexIndex + 3] += offset;
        }

        for (int i = 0; i < _textInfo.meshInfo.Length; i++)
        {
            TMP_MeshInfo meshInfo = _textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;
            _tmpText.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}