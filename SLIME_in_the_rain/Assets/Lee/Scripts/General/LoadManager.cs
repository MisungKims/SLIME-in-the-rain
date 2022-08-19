using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{

    //Start�� OnEnable�� �ҽ� ���� �����̶� �̱��� ����
    //EditorSceneManager.GetSceneManagerSetup /SceneSetup ������Ʈ ����Ʈ�� ���� �� �ֽ��ϴ�.
    //�׸��� �̸� �� ������ ������ ��Ÿ ������ ���Ҿ� ScriptableObject ������ ����ȭ�� �� �ֽ��ϴ�.
    //������ �����Ϸ��� SceneSetups ����Ʈ�� �ٽ� �����ϰ� EditorSceneManager.RestoreSceneManagerSetup�� Ȱ���Ͻʽÿ�.

    //��Ÿ�� ���� �ε�� ���� ����Ʈ�� �������� sceneCount�� ���� �� GetSceneAt�� ���� �ݺ������� �����մϴ�.

    //���� ������Ʈ�� ���� ���� GameObject.scene���� ���� �� ������ SceneManager.MoveGameObjectToScene�� Ȱ���� ���� ������Ʈ�� ���� ��Ʈ�� �ű� �� �ֽ��ϴ�.

    //������ ���ܵα� ���ϴ� ���� ������Ʈ�� ���������� �����ϴ� �� DontDestroyOnLoad�� ����ϴ� ���� �������� �ʽ��ϴ�.
    //��� ��� �Ŵ����� �ִ� manager scene�� �����ϰ� ���� ���μ����� ������ �� SceneManager.LoadScene(<path>, LoadSceneMode.Additive)��SceneManager.UnloadScene�� Ȱ���Ͻʽÿ�.
    // Start is called before the first frame update
       void Start()
    {
        
    }

    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
        //SceneManager.LoadScene(< path >, LoadSceneMode.Additive)
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
