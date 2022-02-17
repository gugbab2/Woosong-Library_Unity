using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
namespace agora_utilities
{
    //드래그 앤 드랍
    public class UIElementDragFullsize : EventTrigger,IPointerClickHandler
    {
        bool IDCardChk = false;
        List<RaycastResult> results = new List<RaycastResult>();
        RectTransform rectTran;
        Vector2 previousposition;
        float clickTime = 0;
        public override void OnDrag(PointerEventData eventData)
        {
            transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            base.OnDrag(eventData);
        }
        public GraphicRaycaster gr;
        private void Awake()
        {
            gr = GetComponent<GraphicRaycaster>();
            rectTran = gameObject.GetComponent<RectTransform>();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            if ((Time.time - clickTime) < 0.3f)
            {  
                clickTime = -1;
                var ped = new PointerEventData(null);
                ped.position = Input.mousePosition;
                
                gr.Raycast(ped, results);

                if (results.Count <= 0) return;
                // 이벤트 처리부분
                results[0].gameObject.transform.position = ped.position;
                Debug.Log(results[0]);
                
                
               
                Vector3 position = results[0].gameObject.transform.localPosition;
                if (IDCardChk == false)
                {
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1600);
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1080);
                    position.x = -955;
                    position.y = -530;
                    results[0].gameObject.transform.localPosition = position;
                    IDCardChk = true;
                }
                else
                {
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
                    rectTran.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 250);
                    position.x = previousposition.x;
                    position.y = previousposition.y-150;
                    results[0].gameObject.transform.localPosition = position;
                    IDCardChk = false;
                }
                
            }
            else
            {
                clickTime = Time.time;
                if(IDCardChk==false)
                    previousposition = new Vector2(results[0].gameObject.transform.position.x,results[0].gameObject.transform.position.y);
            }
        }
    }
}