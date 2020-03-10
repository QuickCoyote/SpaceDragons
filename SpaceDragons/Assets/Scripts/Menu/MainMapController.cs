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

    private void FixedUpdate()
    {
        shortestdistanceReadout.text = Mathf.CeilToInt(TrackingManager.Instance.ReturnDistanceToTracker()).ToString() + "au";
        highlightIcon.SetActive(true);
    }
    
    public void SetTracker()
    {
        Touch touch = Input.GetTouch(0);
        var mousePos = Input.mousePosition;
        Vector2 pos = Vector2.zero;
        GetPositionOnImage01(map, mousePos, out pos);
        pos /= (map.sprite.rect.width * .5f); //Get Percentage
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
                ptLocationRelativeToImage01.x = (ptLocationRelativeToImage01.x * uiImageObject.sprite.rect.width) - (uiImageObject.sprite.rect.width * 0.5f);
                ptLocationRelativeToImage01.y = (ptLocationRelativeToImage01.y * uiImageObject.sprite.rect.height) - (uiImageObject.sprite.rect.height * 0.5f);

            }
            return true;
        }
        return false;
    }
}
