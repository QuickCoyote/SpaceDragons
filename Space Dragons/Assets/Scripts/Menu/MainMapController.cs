using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMapController : UIBaseClass
{
    public Camera mainMapCam = null;

    public TextMeshProUGUI shortestdistanceReadout = null;
    public GameObject highlightIcon;
    public Vector3 highlightPrevPos = Vector3.zero;
    public Image map;

    float mapX = 1000f;
    float mapY = 1000f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    Vector3 panStart;

    private void Update()
    {
        shortestdistanceReadout.text = Mathf.CeilToInt(TrackingManager.Instance.ReturnDistanceToTracker()).ToString() + "au";
        // This is panning/zooming on the map
        if (UICanvas.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                panStart = mainMapCam.ScreenToWorldPoint(Input.mousePosition);
            }
            #region Zoom
            //if (Input.touchCount == 2)
            //{
            //    Touch touchOne = Input.GetTouch(0);
            //    Touch touchTwo = Input.GetTouch(1);

            //    Vector2 mod = Vector2.zero;

            //    mod.x -= Screen.width / 2;
            //    mod.x += (Screen.width * 0.1469594595f);

            //    mod.y -= Screen.height / 2;
            //    mod.y += (Screen.height * 0.0034722222f);

            //    touchOne.position -= mod;
            //    touchTwo.position -= mod;

            //    touchOne.position *= 0.825f;
            //    touchTwo.position *= 0.825f;

            //    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            //    Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;

            //    float prevMagnitude = (touchOnePrevPos - touchTwoPrevPos).magnitude;
            //    float curMagnitude = (touchOne.position - touchTwo.position).magnitude;

            //    float difference = curMagnitude - prevMagnitude;

            //    Zoom(difference * 0.125f);
            //}
            #endregion
            else if (Input.GetMouseButton(0))
            {
                Vector3 panDirection = panStart - mainMapCam.ScreenToWorldPoint(Input.mousePosition);
                mainMapCam.transform.position += panDirection;
            }

            if (maxX < minX)
            {
                maxX *= -1;
                minX *= -1;
            }
            if (maxY < minY)
            {
                maxY *= -1;
                minY *= -1;
            }
            var v3 = mainMapCam.transform.position;
            v3.x = Mathf.Clamp(v3.x, minX, maxX);
            v3.y = Mathf.Clamp(v3.y, minY, maxY);
            mainMapCam.transform.position = v3;
            if (mainMapCam.orthographicSize > 507)
            {
                mainMapCam.orthographicSize = 510;
                GenerateBounds();
                mainMapCam.transform.position = new Vector3(10, -10, mainMapCam.transform.position.z);
            }
        }
        highlightIcon.SetActive(true);
    }

    //public void Zoom(float increment)
    //{
    //    mainMapCam.orthographicSize = Mathf.Clamp(mainMapCam.orthographicSize - increment, 5, 510);
    //    GenerateBounds();
    //}


    public void GenerateBounds()
    {
        var vertExtent = mainMapCam.orthographicSize;
        minX = vertExtent - mapX / 2.0f;
        maxX = mapX / 2.0f - vertExtent;
        minY = vertExtent - mapY / 2.0f;
        maxY = mapY / 2.0f - vertExtent;
    }

    //public void SetTracker()
    //{
    //    Touch touch = Input.GetTouch(0);
    //    var mousePos = Input.mousePosition;
    //    mousePos.x -= Screen.width / 2;
    //    mousePos.y -= Screen.height / 2;
    //    //mousePos.x += (Screen.width * (332f / 1919.514f));
    //    //mousePos.y += (Screen.height * 0.0034722222f);
    //    //mousePos *= 1.111111111111111111111111111111f;// this is for 16:9 (1.111111111, 16/9)
    //    mousePos *= 1.3333333333f;// this is for 18:9 (1.333333333, 18/9)
    //    Vector3 newMousePos = new Vector3(0, 0, -200);
    //    Vector3 oldMousePos = new Vector3(mousePos.x, mousePos.y, 100);
    //    Vector3 tempPos;
    //    Debug.DrawLine(oldMousePos, newMousePos, Color.red, 100, false);
    //    tempPos = mainMapCam.transform.position + mousePos;
    //    tempPos.z = -4f;
    //    highlightPrevPos = highlightIcon.transform.position;
    //    highlightIcon.transform.position = tempPos;
    //    highlightIcon.SetActive(true);
    //}
    
    public void SetTracker()
    {
        Touch touch = Input.GetTouch(0);
        var mousePos = Input.mousePosition;
        Vector2 pos = Vector2.zero;
        GetPositionOnImage01(map, mousePos, out pos);
        pos /= 1000.0f; //Get Percentage
        pos.x *= WorldManager.Instance.WorldCorner.position.x; //Relative to World
        pos.y *= WorldManager.Instance.WorldCorner.position.y;

        highlightIcon.transform.position = pos;
    }

    //http://answers.unity.com/answers/1455168/view.html
    // Take an image and a screenPosition. Return the ptLocationRelativeToImage01 relative to the image (where x,y between 0.0 and 1.0 are on the image, values below 0.0 or above 1.0 are outside the image).
    public static bool GetPositionOnImage01(UnityEngine.UI.Image uiImageObject, Vector2 screenPosition, out Vector2 ptLocationRelativeToImage01)
    {
        ptLocationRelativeToImage01 = new Vector2();
        RectTransform uiImageObjectRect = uiImageObject.GetComponent<RectTransform>();
        Vector2 localCursor = new Vector2();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(uiImageObjectRect, screenPosition, null, out localCursor))
        {
            Vector2 ptPivotCancelledLocation = new Vector2(localCursor.x - uiImageObjectRect.rect.x, localCursor.y - uiImageObjectRect.rect.y);
            Vector2 ptLocationRelativeToImageInScreenCoordinates = new Vector2();
            // How do we get the location of the image? Calculate the size of the image, then use the pivot information.
            if (uiImageObject.preserveAspect)
            {
                // If we are preserving the aspect ratio of the image, then we need to do some additional calculations
                // Figure out if the image constrained by height or by width.
                float fImageAspectRatio = uiImageObject.sprite.rect.height / uiImageObject.sprite.rect.width;
                float fRectAspectRatio = uiImageObjectRect.rect.height / uiImageObjectRect.rect.width;
                Rect imageRectInLocalScreenCoordinates = new Rect();
                if (fImageAspectRatio > fRectAspectRatio)
                {
                    // The image is constrained by its height.
                    float fImageWidth = (fRectAspectRatio / fImageAspectRatio) * uiImageObjectRect.rect.width;
                    float fExcessWidth = uiImageObjectRect.rect.width - fImageWidth;
                    imageRectInLocalScreenCoordinates.Set(uiImageObjectRect.pivot.x * fExcessWidth, 0, uiImageObjectRect.rect.height / fImageAspectRatio, uiImageObjectRect.rect.height);
                }
                else
                {
                    // The image is constrained by its width.
                    float fImageHeight = (fImageAspectRatio / fRectAspectRatio) * uiImageObjectRect.rect.height;
                    float fExcessHeight = uiImageObjectRect.rect.height - fImageHeight;
                    imageRectInLocalScreenCoordinates.Set(0, uiImageObjectRect.pivot.y * fExcessHeight, uiImageObjectRect.rect.width, fImageAspectRatio * uiImageObjectRect.rect.width);
                }
                ptLocationRelativeToImageInScreenCoordinates.Set(ptPivotCancelledLocation.x - imageRectInLocalScreenCoordinates.x, ptPivotCancelledLocation.y - imageRectInLocalScreenCoordinates.y);
                ptLocationRelativeToImage01.Set(ptLocationRelativeToImageInScreenCoordinates.x / imageRectInLocalScreenCoordinates.width, ptLocationRelativeToImageInScreenCoordinates.y / imageRectInLocalScreenCoordinates.height);
            }
            else
            {
                ptLocationRelativeToImageInScreenCoordinates.Set(ptPivotCancelledLocation.x, ptPivotCancelledLocation.y);
                ptLocationRelativeToImage01.Set((ptLocationRelativeToImageInScreenCoordinates.x / uiImageObjectRect.rect.width), (ptLocationRelativeToImageInScreenCoordinates.y / uiImageObjectRect.rect.height));
                ptLocationRelativeToImage01.x = (ptLocationRelativeToImage01.x * uiImageObject.sprite.rect.width) - (uiImageObject.sprite.rect.width / 2);
                ptLocationRelativeToImage01.y = (ptLocationRelativeToImage01.y * uiImageObject.sprite.rect.height) - (uiImageObject.sprite.rect.height / 2);

            }
            return true;
        }
        return false;
    }
}
