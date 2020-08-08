using BWolf.Utilities.LerpValue;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BWolf.Games.PutUnder
{
    using Move = BoardManager.Move;
    using Board = BoardManager.Board;

    public class Pawn : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image imgCharge = null;

        [SerializeField]
        private Image imgWait = null;

        [SerializeField]
        private TMP_Text txtTime = null;

        [SerializeField]
        private Arrows arrows;

        public Vector2Int GridPosition { get; private set; }

        private List<float> timeStamps;

        private int player;
        private bool pickingTimeStamp;
        private bool charging;

        private float currentWait;

        public bool IsBusy
        {
            get { return pickingTimeStamp || charging; }
        }

        private void Start()
        {
            txtTime.text = null;
            arrows.SetActive(false);

            timeStamps = new List<float>(BoardManager.Instance.TimeStamps);
        }

        private void Update()
        {
            if (!IsBusy)
            {
                SetCurrentWait(currentWait + Time.deltaTime);
            }
        }

        public void DoMove(Board board, Move move)
        {
            Vector2Int newPos;
            if (board.TryGetShift(this, move.Shift, out newPos))
            {
                SetCurrentWait(0);

                FetchTimeStamp(time =>
                {
                    Vector3 worldPosition = board.GetHole(newPos).position;
                    StartCoroutine(ChargeMove(time, worldPosition, newPos));
                });
            }
        }

        public void PlaceAsPlayer(int player, Vector3 worldPosition, Vector2Int gridPosition)
        {
            this.player = player;

            GridPosition = gridPosition;
            transform.position = worldPosition;
        }

        private void FetchTimeStamp(Action<float> onFetch)
        {
            StartCoroutine(ChooseTimeStamp(onFetch));
        }

        private int GetMiddleTimeStampIndex()
        {
            if (timeStamps.Count == 0)
            {
                timeStamps.AddRange(BoardManager.Instance.TimeStamps);
            }
            int count = timeStamps.Count;
            return count % 2 != 0 ? Mathf.CeilToInt(count / 2f) : Mathf.RoundToInt(count / 2f);
        }

        private void SetCurrentWait(float newCurrentWait)
        {
            currentWait = newCurrentWait;

            float wait = BoardManager.Instance.Wait;
            imgWait.fillAmount = Mathf.Clamp(currentWait / wait, 0, wait);
        }

        private void SetTimeStamp(ref int index)
        {
            index = index < 0 ? timeStamps.Count - 1 : index == timeStamps.Count ? 0 : index;
            txtTime.text = timeStamps[index].ToString("#");
        }

        private void ResetTimeStamp(int indexOfUsed)
        {
            timeStamps.RemoveAt(indexOfUsed);
            txtTime.text = null;
        }

        private IEnumerator ChooseTimeStamp(Action<float> onFetch)
        {
            pickingTimeStamp = true;
            arrows.SetActive(true);

            yield return null;

            int index = GetMiddleTimeStampIndex();
            SetTimeStamp(ref index);

            string scroll = string.Empty;
            Move[] moves = player == 1 ? Move.PlayerOne : player == 2 ? Move.PlayerTwo : null;

            do
            {
                foreach (Move move in moves)
                {
                    if (Input.GetKeyDown(move.Key))
                    {
                        scroll = move.Scroll;
                        switch (move.Scroll)
                        {
                            case "left":
                                index--;
                                SetTimeStamp(ref index);
                                break;

                            case "right":
                                index++;
                                SetTimeStamp(ref index);
                                break;

                            default:
                                break;
                        }
                    }
                }

                yield return null;
            }
            while (scroll != "up");

            onFetch(timeStamps[index]);
            ResetTimeStamp(index);
            arrows.SetActive(false);
            pickingTimeStamp = false;
        }

        private IEnumerator ChargeMove(float time, Vector3 worldPosition, Vector2Int gridPosition)
        {
            charging = true;

            LerpValue<float> fill = new LerpValue<float>(0, 1, time);
            while (fill.Continue(Time.deltaTime))
            {
                imgCharge.fillAmount = Mathf.Lerp(fill.Start, fill.End, fill.Perc);
                yield return null;
            }

            transform.position = worldPosition;
            GridPosition = gridPosition;
            imgCharge.fillAmount = 0;

            charging = false;
        }

        [Serializable]
        private struct Arrows
        {
#pragma warning disable 0649

            [SerializeField]
            private Transform root;

#pragma warning restore 0649

            private Image left;
            private Image right;

            public void Init()
            {
                left = root?.GetChild(1).GetComponent<Image>();
                right = root?.GetChild(0).GetComponent<Image>();
            }

            public void SetActive(bool value)
            {
                foreach (Transform arrow in root)
                {
                    arrow.gameObject.SetActive(value);
                }
            }
        }
    }
}