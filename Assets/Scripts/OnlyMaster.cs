using Photon.Pun;
using UnityEngine;

public class OnlyMaster : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("ConfigObject"))
        {
            MasterManager.Instance.SetCanOpenPanel(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("ConfigObject"))
        {
            MasterManager.Instance.SetCanOpenPanel(false);
        }
    }
}
