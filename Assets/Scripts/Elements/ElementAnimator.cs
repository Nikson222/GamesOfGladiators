using System.Collections.Generic;
using DG.Tweening;
using Field;
using ScriptableObjects;
using Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Elements
{
    public class ElementAnimator
    {
        private readonly ElementsSettingsDB _elementsSettingsDB;

        public ElementAnimator(ElementsSettingsDB elementsSettingsDB)
        {
            _elementsSettingsDB = elementsSettingsDB;
        }
        
        public Sequence AnimateAllSpawnElements(List<ElementView> elements, List<Vector3> positions = null)
        {
            Debug.Log("Started AnimateAllSpawnElements");
            var sequence = DOTween.Sequence();
            
            var animationTime1 = 0.5f;
            var animationTime2 = 0.5f;

            var elementCount = elements.Count;
            
            foreach (var element in elements)
            {
                element.transform.SetAsLastSibling();
            }

            for (int i = 0; i < elementCount/2; i++)
            {
                Canvas.ForceUpdateCanvases();

                var endPosition = positions != null ? positions[i] : elements[i].transform.localPosition;
                
                sequence.Join(elements[i].transform.DOLocalMove(Vector3.zero, 0f).SetEase(Ease.InOutQuad));
                sequence.Join(elements[i].transform.DOScale(Vector3.zero, 0f).SetEase(Ease.InOutQuad));
                
                sequence.Join(elements[i].transform.DOLocalMove(endPosition, animationTime1).SetEase(Ease.InOutQuad));
                sequence.Join(elements[i].transform.DOScale(Vector3.one, animationTime1).SetEase(Ease.InOutQuad));
                
                animationTime1 += 0.05f;
            }

            for (int i = elementCount-1; i >= elementCount / 2; i--)
            {
                Canvas.ForceUpdateCanvases();

                var endPosition = elements[i].transform.localPosition;
                
                sequence.Join(elements[i].transform.DOLocalMove(Vector3.zero, 0f).SetEase(Ease.InOutQuad));
                sequence.Join(elements[i].transform.DOScale(Vector3.zero, 0f).SetEase(Ease.InOutQuad));
                
                sequence.Join(elements[i].transform.DOLocalMove(endPosition, animationTime2).SetEase(Ease.InOutQuad));
                sequence.Join(elements[i].transform.DOScale(Vector3.one, animationTime2).SetEase(Ease.InOutQuad));
                
                animationTime2 += 0.05f;
            }
            
            sequence.Append(elements[0].transform.DOScale(elements[0].transform.localScale, 0.3f).SetEase(Ease.InOutQuad));
            return sequence;
        }
        
        public Sequence AnimateAllHideElements(List<FieldView.ElementViewType> elements)
        {
            var sequence = DOTween.Sequence();
            
            // var animationTime1 = 0.5f;
            // var animationTime2 = 0.5f;
            //
            // var elementCount = elements.Count;
            //
            // for (int i = 0; i < elementCount/2; i++)
            // {
            //     Canvas.ForceUpdateCanvases();
            //
            //     sequence.Join(elements[i].View.transform.DOLocalMove(Vector3.zero, animationTime1).SetEase(Ease.InOutQuad));
            //     sequence.Join(elements[i].View.transform.DOScale(Vector3.zero, animationTime1).SetEase(Ease.InOutQuad));
            //     
            //     animationTime1 += 0.05f;
            // }
            //
            // for (int i = elementCount-1; i >= elementCount / 2; i--)
            // {
            //     Canvas.ForceUpdateCanvases();
            //
            //     sequence.Join(elements[i].View.transform.DOLocalMove(Vector3.zero, animationTime2).SetEase(Ease.InOutQuad));
            //     sequence.Join(elements[i].View.transform.DOScale(Vector3.zero, animationTime2).SetEase(Ease.InOutQuad));
            //     
            //     animationTime2 += 0.05f;
            // }
            //
            //sequence.Append(elements[0].View.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutQuad));
            //Canvas.ForceUpdateCanvases();
            return sequence;
        }

        public Sequence AnimateScaleWinningElements(List<FieldView.ElementViewType> elements, List<ElementsTypeEnum> newElementssignal)
        {
            Sequence sequence1 = DOTween.Sequence();
            foreach (var element in elements)
            {
                element.View.SetWinSprite(_elementsSettingsDB.GetElementStyleSettings(element.Type).winSprite);
                
                sequence1.Join(element.View.transform.DOScale(new Vector3(1.7f, 1.7f, 1.7f), 0.2f).
                    SetEase(Ease.InOutQuad));
            }

            Sequence sequence2 = DOTween.Sequence();
            
            foreach (var element in elements)
            {
                sequence2.Join(element.View.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad));
            }

            sequence2.Pause();
            sequence2.SetDelay(2f);
            
            Sequence sequence3 = DOTween.Sequence();

            sequence2.onComplete += () =>
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    elements[i].View.SetSprite(_elementsSettingsDB.GetElementStyleSettings(newElementssignal[i]).sprite);
                }
            };
            
            foreach (var t in elements)
            {
                sequence3.Join(t.View.transform.DOScale(Vector3.zero, 0f).SetEase(Ease.InOutQuad));
                
                sequence3.Join(t.View.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad));
                sequence3.Join(t.View.transform.DOScale(Vector3.one, 1f).SetEase(Ease.InOutQuad));
            }
            
            sequence3.Pause();

            sequence1.onComplete += () => sequence2.Play();
            sequence2.onComplete += () => sequence3.Play();
            
            return sequence3;
        }
    }
}