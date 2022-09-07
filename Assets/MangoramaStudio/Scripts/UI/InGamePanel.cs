using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace MangoramaStudio.Scripts.UI
{
    public class InGamePanel : UIPanel
    {
        [SerializeField] private Text _scoreText;
        private int _score = 0;
        public override void Initialize(UIManager uiManager)
        {
            base.Initialize(uiManager);
            PopulatePointTextView(0);
            GameManager.EventManager.OnEarnPoint += PopulatePointTextView;
        }

        private void OnDestroy()
        {
            GameManager.EventManager.OnEarnPoint -= PopulatePointTextView;
        }

        private void PopulatePointTextView(int value)
        {
            _score += value;
            
            _scoreText.text = "SCORE : " + _score;
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}