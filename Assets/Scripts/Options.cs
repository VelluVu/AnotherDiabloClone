using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class Options : MonoBehaviour
{
    string saveName = "/options.dat";
    public bool optionsOpen;
    public GameObject optionsObject;
    public AudioMixer audioMixer;
    public AudioMixerGroup master;
    public AudioMixerGroup music;
    public AudioMixerGroup sound;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider soundSlider;
    public Button applyButton;
    public Button backButton;

    // Start is called before the first frame update
    void Start()
    {
        applyButton.onClick.AddListener(() => DoOnClick("Apply"));
        backButton.onClick.AddListener(() => DoOnClick("Back"));
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (optionsOpen)
        {
            SetFloat("Master", masterSlider);
            SetFloat("Music", musicSlider);
            SetFloat("Sound", soundSlider);
        }
    }
    public void DoOnClick(string action)
    {
        if(action == "Apply")
        {
            Save();
            
        }
        else if(action == "Back")
        {
            optionsOpen = false;
            optionsObject.SetActive(false);
            Load();
        }
    }
    void SetFloat(string name,Slider slider)
    {
        audioMixer.SetFloat(name, slider.value);
    }
    float GetFloat(string name)
    {
        float temp;
        audioMixer.GetFloat(name, out temp);
        return temp;
    }
    public void Save()
    {


        Debug.Log("Saved Options");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + saveName);
        SaveOptions data = new SaveOptions();

        data.masterVolume = GetFloat("Master");
        data.musicVolume = GetFloat("Music");
        data.soundVolume = GetFloat("Sound");








        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + saveName))
        {
            Debug.Log("<color=red>" + "OptionsLoaded" + "</color>");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + saveName, FileMode.Open);
            SaveOptions data = (SaveOptions)bf.Deserialize(file);
            file.Close();
            masterSlider.value = data.masterVolume;
            musicSlider.value = data.musicVolume;
            soundSlider.value = data.soundVolume;
            SetFloat("Master", masterSlider);
            SetFloat("Music", musicSlider);
            SetFloat("Sound", soundSlider);



        }
    }
}

[Serializable]
class SaveOptions
{
    public float masterVolume;
    public float musicVolume;
    public float soundVolume;
    
}
