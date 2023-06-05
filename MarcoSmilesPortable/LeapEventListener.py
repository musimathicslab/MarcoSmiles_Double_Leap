import Leap
import math
import sys
from predict import load_utils, make_prediction
import pandas as pd


# https://developer-archive.leapmotion.com/documentation/python/devguide/Leap_Controllers.html

class LeapEventListener(Leap.Listener):
    def on_connect(self, controller):
        print("Leap Motion connected")

    def on_disconnect(self, controller):
        print("Leap Motion disconnected")

    def on_frame(self, controller):

        connected_devices = len(controller.devices)
        print("Connected devices: " + connected_devices)

        frame = controller.frame()

        features_values_left = []
        features_values_right = []
        features_values = []

        # Scan all connected devices
        for device in controller.devices:
            # Get only hands of a specific device
            hands = [hand for hand in frame.hands if hand.device == device]
            # Check for each device connected if there are two hands
            if len(hands) == 2:
                for hand in hands:
                    if hand.is_left:
                        features_values_left = get_features(features_values_left, hand)
                    elif hand.is_right:
                        features_values_right = get_features(features_values_right, hand)

                features_values_tmp = features_values_left + features_values_right
                features_values += features_values_tmp

        # Perform prediction only when we have 36 features
        if len(features_values) != 0 and len(features_values) == 36 :
            # Predict
            features_values = pd.Series(features_values)
            prediction = make_prediction(features_values)
            print(prediction)


# This function take in input a hand and compute two values: FF and NFA
def get_features(features_values, hand):
    for i in range(0, 5):
        if i == 0:
            features_values.append(get_FF(hand.fingers[i], True))
        else:
            features_values.append(get_FF(hand.fingers[i]))

    for i in range(0, 4):
        features_values.append(get_NFA(hand.fingers[i], hand.fingers[i + 1]))

    return features_values


def get_FF(f, is_thumb=False):
    if is_thumb:
        return grads(math.degrees(f.bone(1).direction.angle_to(f.bone(3).direction)))
    else:
        return grads(math.degrees(f.bone(0).direction.angle_to(f.bone(3).direction)))


def get_NFA(f1, f2):
    return grads(math.degrees(f1.direction.angle_to(f2.direction)))


def grads(num):
    return num * 180.0 / math.pi


def main():
    load_utils()

    # Create a listener and controller
    listener = LeapEventListener()
    controller = Leap.Controller()

    # Have the sample listener receive events from the controller
    controller.add_listener(listener)

    # Keep this process running until Enter is pressed
    print("Press Enter to quit...")
    try:
        sys.stdin.readline()
    except KeyboardInterrupt:
        pass
    finally:
        # Remove the sample listener when done
        controller.remove_listener(listener)


if __name__ == "__main__":
    main()
