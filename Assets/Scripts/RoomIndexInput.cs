using UnityEngine;
using UnityEngine.InputSystem;

public class RoomIndexInput : MonoBehaviour
{
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.digit1Key.wasPressedThisFrame)
        {
            SetRoomIndex(0);
        }
        else if (keyboard.digit2Key.wasPressedThisFrame)
        {
            SetRoomIndex(1);
        }
        else if (keyboard.digit3Key.wasPressedThisFrame)
        {
            SetRoomIndex(2);
        }
        else if (keyboard.digit4Key.wasPressedThisFrame)
        {
            SetRoomIndex(3);
        }
        else if (keyboard.digit5Key.wasPressedThisFrame)
        {
            SetRoomIndex(4);
        }
    }

    void SetRoomIndex(int index)
    {
        if (AudioController.Instance == null)
        {
            Debug.LogWarning("RoomIndexInput: AudioController.Instance is null.");
            return;
        }

        AudioController.Instance.SetLevelAudio(index);
        Debug.Log("Room audio index set to: " + index);
    }
}
