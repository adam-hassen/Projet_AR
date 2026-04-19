using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class FirstAidSteps : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        public string title;
        [TextArea] public string instruction;
        public bool hasARDemo;
        public bool hasAnimatedGuide;
    }

    public Step[] steps;
    public GameObject menuPanel;
    public GameObject instructionPanel;
    public TextMeshProUGUI instructionTitle;
    public TextMeshProUGUI instructionText;
    public Transform stepsContainer;
    public GameObject stepButtonPrefab;

    private PlaceObject placer;
    private int currentStepIndex = 0;
    private GameObject securityGuidePanel;

    void Start()
    {
        placer = FindObjectOfType<PlaceObject>();

        SetFullScreen(menuPanel);
        SetFullScreen(instructionPanel);

        foreach (Transform child in instructionPanel.transform)
        {
            string n = child.name;
            if (n == "NextButton" || n == "PrevButton" || n == "StepCounterText")
                child.gameObject.SetActive(false);
        }

        if (instructionTitle != null)
        {
            RectTransform rt = instructionTitle.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0.85f);
            rt.anchorMax = new Vector2(1f, 0.97f);
            rt.offsetMin = new Vector2(20f, 0f);
            rt.offsetMax = new Vector2(-20f, 0f);
        }

        if (instructionText != null)
        {
            RectTransform rt = instructionText.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0.35f);
            rt.anchorMax = new Vector2(1f, 0.84f);
            rt.offsetMin = new Vector2(30f, 0f);
            rt.offsetMax = new Vector2(-30f, 0f);
        }

        CreateInstructionButtons();
        CreateSecurityGuidePanel();

        menuPanel.GetComponent<Image>().color = new Color(0.04f, 0.04f, 0.18f, 0.97f);
        instructionPanel.GetComponent<Image>().color = new Color(0.04f, 0.04f, 0.18f, 0.97f);

        steps = new Step[]
        {
            new Step { title = "Securite", instruction = "", hasARDemo = false, hasAnimatedGuide = true },
            new Step { title = "Conscience", instruction = "Verifiez si la victime reagit :\n\n1. Tapotez les epaules doucement\n2. Criez : Vous m entendez ?\n3. Si pas de reponse, appelez le 15\n4. Verifiez si elle respire", hasARDemo = false, hasAnimatedGuide = false },
            new Step { title = "Massage Cardiaque", instruction = "Effectuez le massage :\n\n1. Mains au centre du thorax\n2. Bras tendus, appuyez fort\n3. 30 compressions rapides\n4. Continuez jusqu aux secours", hasARDemo = true, hasAnimatedGuide = false },
            new Step { title = "Appel Secours", instruction = "Appelez immediatement :\n\n- 15  SAMU\n- 18  Pompiers\n- 112  Numero europeen\n\nDonnez votre position exacte", hasARDemo = false, hasAnimatedGuide = false }
        };

        menuPanel.SetActive(true);
        instructionPanel.SetActive(false);
        BuildMenu();
    }

    void CreateSecurityGuidePanel()
{
    securityGuidePanel = new GameObject("SecurityGuide");
    securityGuidePanel.transform.SetParent(instructionPanel.transform, false);
    RectTransform rt = securityGuidePanel.AddComponent<RectTransform>();
    rt.anchorMin = new Vector2(0f, 0.15f);
    rt.anchorMax = new Vector2(1f, 0.84f);
    rt.offsetMin = new Vector2(15f, 0f);
    rt.offsetMax = new Vector2(-15f, 0f);
    securityGuidePanel.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);

    // 4 etapes de securite en liste
    string[] steps_sec = {
        "1.  REGARDEZ autour de vous",
        "2.  ECARTEZ tout danger",
        "3.  NE BOUGEZ PAS la victime",
        "4.  PROTEGEZ-VOUS d'abord"
    };
    Color[] stepColors = {
        new Color(0.9f, 0.1f, 0.1f, 1f),
        new Color(0.95f, 0.4f, 0.02f, 1f),
        new Color(0.85f, 0.75f, 0f, 1f),
        new Color(0.08f, 0.65f, 0.25f, 1f)
    };

    // Message titre
    GameObject msgObj = new GameObject("MainMsg");
    msgObj.transform.SetParent(securityGuidePanel.transform, false);
    TextMeshProUGUI msg = msgObj.AddComponent<TextMeshProUGUI>();
    msg.text = "AVANT TOUT GESTE :";
    msg.fontSize = 32;
    msg.color = new Color(1f, 0.9f, 0f, 1f);
    msg.fontStyle = FontStyles.Bold;
    msg.alignment = TextAlignmentOptions.Center;
    RectTransform msgRt = msgObj.GetComponent<RectTransform>();
    msgRt.anchorMin = new Vector2(0f, 0.87f);
    msgRt.anchorMax = new Vector2(1f, 1f);
    msgRt.offsetMin = Vector2.zero;
    msgRt.offsetMax = Vector2.zero;

    // 4 cartes d'etapes
    for (int i = 0; i < 4; i++)
    {
        int idx = i;
        float yPos = 0.64f - (i * 0.22f);

        // Carte
        GameObject card = new GameObject("Step" + i);
        card.transform.SetParent(securityGuidePanel.transform, false);
        Image cardImg = card.AddComponent<Image>();
        cardImg.color = stepColors[i];
        RectTransform cardRt = card.GetComponent<RectTransform>();
        cardRt.anchorMin = new Vector2(0f, yPos);
        cardRt.anchorMax = new Vector2(1f, yPos + 0.19f);
        cardRt.offsetMin = new Vector2(0f, 4f);
        cardRt.offsetMax = new Vector2(0f, -4f);

        // Texte de l'etape
        GameObject txtObj = new GameObject("StepTxt");
        txtObj.transform.SetParent(card.transform, false);
        TextMeshProUGUI txt = txtObj.AddComponent<TextMeshProUGUI>();
        txt.text = steps_sec[i];
        txt.fontSize = 32;
        txt.fontStyle = FontStyles.Bold;
        txt.alignment = TextAlignmentOptions.Left;
        txt.color = Color.white;
        RectTransform txtRt = txtObj.GetComponent<RectTransform>();
        txtRt.anchorMin = new Vector2(0f, 0f);
        txtRt.anchorMax = new Vector2(1f, 1f);
        txtRt.offsetMin = new Vector2(20f, 0f);
        txtRt.offsetMax = new Vector2(-10f, 0f);

        StartCoroutine(PulseCard(cardImg, stepColors[i], i * 0.4f));
    }

    // Dangers possibles en bas
    GameObject dangerTitle = new GameObject("DangerTitle");
    dangerTitle.transform.SetParent(securityGuidePanel.transform, false);
    TextMeshProUGUI dTxt = dangerTitle.AddComponent<TextMeshProUGUI>();
    dTxt.text = "DANGERS : Voitures  |  Feu  |  Electricite  |  Gaz";
    dTxt.fontSize = 24;
    dTxt.color = new Color(0.8f, 0.8f, 1f, 1f);
    dTxt.fontStyle = FontStyles.Italic;
    dTxt.alignment = TextAlignmentOptions.Center;
    RectTransform dRt = dangerTitle.GetComponent<RectTransform>();
    dRt.anchorMin = new Vector2(0f, 0f);
    dRt.anchorMax = new Vector2(1f, 0.10f);
    dRt.offsetMin = Vector2.zero;
    dRt.offsetMax = Vector2.zero;

    securityGuidePanel.SetActive(false);
}

IEnumerator PulseCard(Image card, Color baseColor, float delay)
{
    yield return new WaitForSeconds(delay);
    while (true)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            float pulse = 0.85f + Mathf.Abs(Mathf.Sin(t * Mathf.PI)) * 0.15f;
            card.color = new Color(baseColor.r * pulse, baseColor.g * pulse, baseColor.b * pulse, 1f);
            yield return null;
        }
    }
}

    IEnumerator AnimateArrowImage(GameObject arrowObj, Image arrowImg, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector2 startPos = arrowObj.GetComponent<RectTransform>().anchoredPosition;
        while (true)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 2.5f;
                float bounce = Mathf.Abs(Mathf.Sin(t * Mathf.PI));
                float alpha = 0.5f + bounce * 0.5f;
                arrowImg.color = new Color(1f, 1f, 1f, alpha);
                arrowObj.GetComponent<RectTransform>().anchoredPosition =
                    startPos + new Vector2(0f, -bounce * 12f);
                yield return null;
            }
        }
    }

    void SetFullScreen(GameObject panel)
    {
        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    void CreateInstructionButtons()
    {
        if (instructionPanel.transform.Find("ARBtn") != null) return;

        // Bouton AR bleu
        GameObject arBtn = new GameObject("ARBtn");
        arBtn.transform.SetParent(instructionPanel.transform, false);
        arBtn.AddComponent<Image>().color = new Color(0.1f, 0.4f, 0.9f, 1f);
        Button arButton = arBtn.AddComponent<Button>();
        arButton.onClick.AddListener(() => {
            placer.ActivateARMode();
            instructionPanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            if (instructionTitle != null)
            {
                RectTransform rt = instructionTitle.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0f, 0.90f);
                rt.anchorMax = new Vector2(1f, 1f);
                rt.offsetMin = new Vector2(10f, 0f);
                rt.offsetMax = new Vector2(-10f, 0f);
            }
            if (instructionText != null)
            {
                RectTransform rt = instructionText.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0f, 0.75f);
                rt.anchorMax = new Vector2(1f, 0.90f);
                rt.offsetMin = new Vector2(10f, 0f);
                rt.offsetMax = new Vector2(-10f, 0f);
            }
            arBtn.SetActive(false);
        });
        RectTransform arRt = arBtn.GetComponent<RectTransform>();
        arRt.anchorMin = new Vector2(0.05f, 0.22f);
        arRt.anchorMax = new Vector2(0.95f, 0.33f);
        arRt.offsetMin = Vector2.zero;
        arRt.offsetMax = Vector2.zero;
        GameObject arTxtObj = new GameObject("ARText");
        arTxtObj.transform.SetParent(arBtn.transform, false);
        TextMeshProUGUI arTxt = arTxtObj.AddComponent<TextMeshProUGUI>();
        arTxt.text = "VOIR LA DEMONSTRATION AR";
        arTxt.fontSize = 34;
        arTxt.color = Color.white;
        arTxt.fontStyle = FontStyles.Bold;
        arTxt.alignment = TextAlignmentOptions.Center;
        RectTransform arTxtRt = arTxtObj.GetComponent<RectTransform>();
        arTxtRt.anchorMin = Vector2.zero;
        arTxtRt.anchorMax = Vector2.one;
        arTxtRt.offsetMin = Vector2.zero;
        arTxtRt.offsetMax = Vector2.zero;

        // Bouton RETOUR rouge
        GameObject retBtn = new GameObject("RetourBtn");
        retBtn.transform.SetParent(instructionPanel.transform, false);
        retBtn.AddComponent<Image>().color = new Color(0.75f, 0.08f, 0.08f, 1f);
        Button retButton = retBtn.AddComponent<Button>();
        retButton.onClick.AddListener(() => {
            placer.DeactivateARMode();
            instructionPanel.GetComponent<Image>().color = new Color(0.04f, 0.04f, 0.18f, 0.97f);
            if (instructionTitle != null)
            {
                RectTransform rt = instructionTitle.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0f, 0.85f);
                rt.anchorMax = new Vector2(1f, 0.97f);
                rt.offsetMin = new Vector2(20f, 0f);
                rt.offsetMax = new Vector2(-20f, 0f);
            }
            if (instructionText != null)
            {
                RectTransform rt = instructionText.GetComponent<RectTransform>();
                rt.anchorMin = new Vector2(0f, 0.35f);
                rt.anchorMax = new Vector2(1f, 0.84f);
                rt.offsetMin = new Vector2(30f, 0f);
                rt.offsetMax = new Vector2(-30f, 0f);
            }
            GameObject ar = instructionPanel.transform.Find("ARBtn")?.gameObject;
            if (ar != null) ar.SetActive(true);
            if (securityGuidePanel != null)
                securityGuidePanel.SetActive(false);
            BackToMenu();
        });
        RectTransform retRt = retBtn.GetComponent<RectTransform>();
        retRt.anchorMin = new Vector2(0.05f, 0.03f);
        retRt.anchorMax = new Vector2(0.95f, 0.14f);
        retRt.offsetMin = Vector2.zero;
        retRt.offsetMax = Vector2.zero;
        GameObject retTxtObj = new GameObject("RetText");
        retTxtObj.transform.SetParent(retBtn.transform, false);
        TextMeshProUGUI retTxt = retTxtObj.AddComponent<TextMeshProUGUI>();
        retTxt.text = "RETOUR AU MENU";
        retTxt.fontSize = 38;
        retTxt.color = Color.white;
        retTxt.fontStyle = FontStyles.Bold;
        retTxt.alignment = TextAlignmentOptions.Center;
        RectTransform retTxtRt = retTxtObj.GetComponent<RectTransform>();
        retTxtRt.anchorMin = Vector2.zero;
        retTxtRt.anchorMax = Vector2.one;
        retTxtRt.offsetMin = Vector2.zero;
        retTxtRt.offsetMax = Vector2.zero;
    }

    void BuildMenu()
    {
        TextMeshProUGUI title = menuPanel.GetComponentInChildren<TextMeshProUGUI>();
        if (title != null)
        {
            title.text = "PREMIERS SECOURS";
            title.fontSize = 62;
            title.color = new Color(1f, 0.15f, 0.15f, 1f);
            title.alignment = TextAlignmentOptions.Center;
            title.fontStyle = FontStyles.Bold;
            RectTransform rt = title.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0.88f);
            rt.anchorMax = new Vector2(1f, 1f);
            rt.offsetMin = new Vector2(20f, 0f);
            rt.offsetMax = new Vector2(-20f, -10f);
        }

        if (menuPanel.transform.Find("Subtitle") == null)
        {
            GameObject subObj = new GameObject("Subtitle");
            subObj.transform.SetParent(menuPanel.transform, false);
            TextMeshProUGUI sub = subObj.AddComponent<TextMeshProUGUI>();
            sub.text = "Selectionnez une etape pour commencer";
            sub.fontSize = 28;
            sub.color = new Color(0.6f, 0.7f, 1f, 1f);
            sub.alignment = TextAlignmentOptions.Center;
            sub.fontStyle = FontStyles.Italic;
            RectTransform rt = subObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0f, 0.80f);
            rt.anchorMax = new Vector2(1f, 0.88f);
            rt.offsetMin = new Vector2(30f, 0f);
            rt.offsetMax = new Vector2(-30f, 0f);
        }

        if (menuPanel.transform.Find("Line") == null)
        {
            GameObject lineObj = new GameObject("Line");
            lineObj.transform.SetParent(menuPanel.transform, false);
            Image line = lineObj.AddComponent<Image>();
            line.color = new Color(1f, 0.2f, 0.2f, 0.8f);
            RectTransform rt = lineObj.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.05f, 0.778f);
            rt.anchorMax = new Vector2(0.95f, 0.784f);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        foreach (Transform child in stepsContainer)
            Destroy(child.gameObject);

        VerticalLayoutGroup layout = stepsContainer.GetComponent<VerticalLayoutGroup>();
        if (layout == null)
            layout = stepsContainer.gameObject.AddComponent<VerticalLayoutGroup>();
        layout.spacing = 22f;
        layout.padding = new RectOffset(20, 20, 20, 20);
        layout.childControlWidth = true;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = true;

        ContentSizeFitter fitter = stepsContainer.GetComponent<ContentSizeFitter>();
        if (fitter == null)
            fitter = stepsContainer.gameObject.AddComponent<ContentSizeFitter>();
        fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        ScrollRect scroll = stepsContainer.GetComponentInParent<ScrollRect>();
        if (scroll != null)
        {
            RectTransform scrollRect = scroll.GetComponent<RectTransform>();
            scrollRect.anchorMin = new Vector2(0f, 0.05f);
            scrollRect.anchorMax = new Vector2(1f, 0.77f);
            scrollRect.offsetMin = new Vector2(20f, 0f);
            scrollRect.offsetMax = new Vector2(-20f, 0f);
        }

        string[] nums = { "1", "2", "3", "4" };
        string[] titles = { "SECURITE", "CONSCIENCE", "MASSAGE CARDIAQUE", "APPEL SECOURS" };
        string[] subs = {
            "Guide interactif des dangers",
            "Verifier la victime",
            "Demonstration AR disponible",
            "Appeler le 15 ou 112"
        };
        Color[] colors = {
            new Color(0.15f, 0.45f, 0.95f, 1f),
            new Color(0.1f, 0.7f, 0.3f, 1f),
            new Color(0.9f, 0.1f, 0.1f, 1f),
            new Color(0.95f, 0.5f, 0.05f, 1f)
        };

        for (int i = 0; i < steps.Length; i++)
        {
            int index = i;
            GameObject btn = Instantiate(stepButtonPrefab, stepsContainer);
            btn.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, 155f);
            btn.GetComponent<Image>().color = colors[i];
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null)
            {
                btnText.text = "  " + nums[i] + ".  " + titles[i]
                             + "\n<size=26><color=#ffffffbb>     " + subs[i] + "</color></size>";
                btnText.fontSize = 40;
                btnText.color = Color.white;
                btnText.fontStyle = FontStyles.Bold;
                btnText.alignment = TextAlignmentOptions.Left;
                btnText.enableWordWrapping = false;
            }
            btn.GetComponent<Button>().onClick.AddListener(() => SelectStep(index));
        }
    }

    public void SelectStep(int index)
    {
        currentStepIndex = index;

        Color[] stepColors = {
            new Color(0.15f, 0.45f, 0.95f, 1f),
            new Color(0.1f, 0.7f, 0.3f, 1f),
            new Color(0.9f, 0.1f, 0.1f, 1f),
            new Color(0.95f, 0.5f, 0.05f, 1f)
        };

        instructionPanel.GetComponent<Image>().color = new Color(0.04f, 0.04f, 0.18f, 0.97f);
        instructionTitle.text = steps[index].title.ToUpper();
        instructionTitle.fontSize = 55;
        instructionTitle.color = stepColors[index % stepColors.Length];
        instructionTitle.fontStyle = FontStyles.Bold;
        instructionTitle.alignment = TextAlignmentOptions.Center;

        GameObject arBtn = instructionPanel.transform.Find("ARBtn")?.gameObject;
        if (arBtn != null)
            arBtn.SetActive(steps[index].hasARDemo);

        if (securityGuidePanel != null)
            securityGuidePanel.SetActive(steps[index].hasAnimatedGuide);

        if (steps[index].hasAnimatedGuide)
        {
            if (instructionText != null)
                instructionText.text = "";
        }
        else
        {
            instructionText.text = steps[index].instruction;
            instructionText.fontSize = 36;
            instructionText.color = Color.white;
            instructionText.alignment = TextAlignmentOptions.Left;
            instructionText.lineSpacing = 12f;
        }

        menuPanel.SetActive(false);
        instructionPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        if (securityGuidePanel != null)
            securityGuidePanel.SetActive(false);
        menuPanel.SetActive(true);
        instructionPanel.SetActive(false);
    }
}