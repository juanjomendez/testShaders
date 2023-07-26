using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class code : MonoBehaviour
{

    public float speed = 5.0f;
    public float sensitivity = 5.0f;
    public Camera camera;

    public Material [] materials;
    public GameObject [] objects;

    bool hoveringAnObject;

    public TextMeshProUGUI text1, text2, text3;
    float t1, t2, t3;

    string defaultT1 = "View complete = ";
    string defaultT2 = "View partially = ";
    string defaultT3 = "Not seen = ";

    Dictionary<string, float> holdObjects = new Dictionary<string, float>();

    void Start()
    {
        hoveringAnObject = false;
    }


    void SetMaterial(GameObject go, Material newMaterial)
    {
        Material[] materialsArray = new Material[(go.GetComponent<MeshRenderer>().materials.Length + 1)];
        go.GetComponent<MeshRenderer>().materials.CopyTo(materialsArray, 0);
        materialsArray[materialsArray.Length - 1] = newMaterial;
        go.GetComponent<MeshRenderer>().materials = materialsArray;
    }

    void removeMaterials(GameObject go)
    {
        go.GetComponent<MeshRenderer>().materials = new Material[0];
    }

    void updateTexts()
    {
        text1.text = defaultT1 + t1.ToString("F2");
        text2.text = defaultT2 + t2.ToString("F2");
        text3.text = defaultT3 + t3.ToString("F2");
    }


    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            camera.transform.position += camera.transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
            camera.transform.position += camera.transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");
            camera.transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
        }
        else
        {

            RaycastHit[] hits;
            hits = Physics.RaycastAll(Camera.main.ScreenPointToRay(Input.mousePosition));

            System.Array.Sort(hits, (a, b) =>(b.distance.CompareTo(a.distance)));

            int i = 0;

            if ((hits.Length == 0) && (hoveringAnObject == true))
            {
                for (int x = 0;x < objects.Length;x++)
                {
                    removeMaterials(objects[x]);
                    SetMaterial(objects[x], materials[0]);
                }
                hoveringAnObject = false;

                holdObjects.Clear();
                t1 = 0f;
                t2 = 0f;
                t3 = 0f;
                updateTexts();
            }

            while (i < hits.Length)
            {
                
                hoveringAnObject = true;

                if (i != (hits.Length - 1))//there is another object on top
                {

                    if (holdObjects.ContainsKey(hits[i].collider.gameObject.name))
                    {
                        float firstDistance;
                        holdObjects.TryGetValue(hits[i].collider.gameObject.name, out firstDistance);

                        float kk = (firstDistance - hits[i].distance);

                        if (kk > 0f)
                        {
                            removeMaterials(hits[i].collider.gameObject);
                            SetMaterial(hits[i].collider.gameObject, materials[3]);
                            t3 += Time.deltaTime;
                            updateTexts();
                        }
                        else
                        {
                            removeMaterials(hits[i].collider.gameObject);
                            SetMaterial(hits[i].collider.gameObject, materials[1]);
                            t2 += Time.deltaTime;
                            updateTexts();

                        }

                    }
                    else
                    {
                        removeMaterials(hits[i].collider.gameObject);
                        SetMaterial(hits[i].collider.gameObject, materials[1]);

                        holdObjects.Add(hits[i].collider.gameObject.name, hits[i].distance);
                    }

                }
                else
                {
                    t1 += Time.deltaTime;
                    updateTexts();
                }

                i++;
            }

        }

    }

}
