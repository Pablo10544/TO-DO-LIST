using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opening : MonoBehaviour
{
    // Start is called before the first frame update
    Image i;
    public int p;
    public Text t;
    public List<string> quotes = new List<string>();
    void Start()
    {
        quotes.Add("He who tries wins.");
        t.text = quotes[0];
        i = GetComponent<Image>();

        StartCoroutine(IamACoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator IamACoroutine() {
        yield return new WaitForSeconds(1.5f);
        while (i.color.a > 0.01f) {
            print(i.color.a);
            i.color = new Color(i.color.r, i.color.g, i.color.b, Mathf.Lerp(i.color.a,0,0.1f/p));
            yield return null;
        }
        this.gameObject.SetActive(false);
       
    }
}
