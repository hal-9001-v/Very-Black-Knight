using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGUI : MonoBehaviour
{
    public GameObject canvasObject;

    //################################
    //HEALTH
    public GameObject levelUpScreenObject;
    public GameObject colorBarObject;
    Image colorBar;
    public GameObject backgroundBarObject;


    float maxHealth;
    Vector3 maxLocalScale;
    //###############################


    //###############################
    //LEVEL
    public GameObject levelTextContainer;
    Text levelText;

    public GameObject upgradesLeftObject;
    private Text upgradesLeftText;

    public Text movementText;
    public Text healthText;

    
    //###############################


    // Start is called before the first frame update
    void Awake()
    {

        colorBar = colorBarObject.GetComponent<Image>();
        colorBar.rectTransform.pivot = new Vector2(0, 0.5f);
        colorBar.fillMethod = Image.FillMethod.Horizontal;

        maxLocalScale = colorBar.rectTransform.localScale;

        levelText = levelTextContainer.GetComponent<Text>();

        upgradesLeftText = upgradesLeftObject.GetComponent<Text>();

        levelUpScreenObject.SetActive(false);
  
    }

    public void setCurrentLevel(int level)
    {
        levelText.text = ""+level;
    }

    public void setMaxHealth(float maxHealth)
    {

        this.maxHealth = maxHealth;
        setHealth(maxHealth);

    }

    public void setHealth(float hp)
    {
        if (hp < 0 | hp > maxHealth)
        {
            setHealth(0);

        }

        Vector3 actualScale = maxLocalScale;
        actualScale.x *= hp / maxHealth;
        
        colorBar.rectTransform.localScale = actualScale;


    }

    public void setLevelUpIndicators(int movementLvl, int healthLvl, int upgrades) {
        movementText.text = "Current Lvl: " + movementLvl;
        healthText.text = "Current Lvl: " + healthLvl;
        upgradesLeftText.text = "UPGRADES LEFT: "+upgrades;

        levelUpScreenObject.SetActive(true);
        canvasObject.GetComponent<GraphicRaycaster>().enabled = true;
    }

    public void hideLevelUpIndicators() {
        levelUpScreenObject.SetActive(false);
        canvasObject.GetComponent<GraphicRaycaster>().enabled = false;
    }
}
