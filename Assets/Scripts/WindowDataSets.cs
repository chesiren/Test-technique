using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class WindowDataSets : MonoBehaviour
{
    [Header("Button")]
    [SerializeField]
    private GameObject buttonPrefab;
    [SerializeField]
    private GameObject buttonParent;

    [Header("Window")]
    [SerializeField] 
    private GameObject windowFrame;
    [SerializeField]
    private GameObject windowTitle;
    [SerializeField]
    private GameObject windowText;


    public class WindowContent
    {
        public int id;
        public string title;
        public string content;
    }

    [Serializable]
    public class DataSetButton
    {
        public string text;
        public string path;
        public List<WindowContent> data;
    }

    [SerializeField]
    private List<DataSetButton> dataSets;

    void Start()
    {
        // Boucle sur les dataSets
        foreach (DataSetButton dataSet in dataSets)
        {
            // Stocker les json désérialisés pour ne pas avoir à le faire à chaque ouverture de fenêtre
            string loadFile = File.ReadAllText(dataSet.path);
            dataSet.data = JsonConvert.DeserializeObject<List<WindowContent>>(loadFile);

            // Créer les boutons dans la sidebar
            GameObject sidebarButton = Instantiate(buttonPrefab, buttonParent.transform);
            sidebarButton.GetComponent<Button>().onClick.AddListener(() => OpenWindow(dataSet.data));
            sidebarButton.GetComponentInChildren<Text>().text = dataSet.text;
        }
    }

    /// <summary>
    /// Ouvre la fenêtre et ajoute les boutons en prenant en paramètre le json désérialisé
    /// </summary>
    /// <param name="data">Json désérialisé</param>
    public void OpenWindow(List<WindowContent> data)
    {
        // Supprimer les boutons crées par le dataset précédent
        windowText.GetComponent<Text>().text = "Clickez sur un bouton pour afficher les données";
        foreach (Transform child in windowTitle.transform)
        {
            Destroy(child.gameObject);
        }

        // Boucle sur chaque classe WindowContent pour récupérer leurs données
        foreach (WindowContent window in data)
        {
            // Créer les boutons dans la fenêtre
            GameObject titleButton = Instantiate(buttonPrefab, windowTitle.transform);
            titleButton.GetComponent<Button>().onClick.AddListener(() => windowText.GetComponent<Text>().text = window.content);
            titleButton.GetComponentInChildren<Text>().text = window.title;
        }

        // Activer la fenêtre
        windowFrame.SetActive(true);
    }
}
