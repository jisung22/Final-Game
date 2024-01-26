using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public int stage;

    public GameObject startZone;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntC;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public GameObject overPanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Text enemyATxt;
    public Text enemyCTxt;
    
    public Text curScoreText;

    public Transform[] enemyZone;
    public GameObject[] enemies;
    public List<int> enemyList;


    private void Awake()
    {
        enemyList = new List<int>();
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if (PlayerPrefs.HasKey("MaxScore"))
            PlayerPrefs.SetInt("MaxScore", 0);
    }
    public void GameStart()
    {
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }
    public void GameOver()
    {
        gamePanel.SetActive(false);
        overPanel.SetActive(true);
        curScoreText.text = scoreTxt.text;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StageStart()
    {
        startZone.SetActive(false);

        foreach (Transform zone in enemyZone)
            zone.gameObject.SetActive(true);

        isBattle = true;
        StartCoroutine(InBattle());
    }
    public void StageEnd()
    {
        startZone.SetActive(true);

        foreach (Transform zone in enemyZone)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;

    }

    IEnumerator InBattle()
    {
        for (int index = 0; index < stage; index++)
        {
            int ran = Random.Range(0, 1);
            enemyList.Add(ran);

            switch (ran)
            {
                case 0:
                    enemyCntA++;
                    break;

                case 1:
                    enemyCntC++;
                    break;

            }
        }

        while (enemyList.Count > 0)
        {
            int ranZone = Random.Range(0, 4);
            GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZone[ranZone].position, enemyZone[ranZone].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.Target = player.transform;
            enemy.manager = this;
            enemyList.RemoveAt(0);
            yield return new WaitForSeconds(4f);

        }
        while (enemyCntA + enemyCntC > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        StageEnd();
        
        
}




private void Update()
    {
        if (isBattle)
            playTime += Time.deltaTime;
    }

    private void LateUpdate()
    {
        scoreTxt.text = string.Format("{0:n0}", player.score);
        stageTxt.text = "Stage" + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600)/60);
        int second = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
        playerHealthTxt.text = player.heart + "/" + player.maxHeart;
        
        if(player.equipWeapon == null)
            playerAmmoTxt.text = "- /" + player.ammo ;
        else if (player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text =  "- /" + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + "- /" + player.ammo;

        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);

        enemyATxt.text = enemyCntA.ToString();
        enemyCTxt.text = enemyCntC.ToString();

    }

}
