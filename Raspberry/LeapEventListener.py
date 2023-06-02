import Leap
import math
import sys


# https://developer-archive.leapmotion.com/documentation/python/devguide/Leap_Controllers.html

class LeapEventListener(Leap.Listener):
    def on_connect(self, controller):
        print("Leap Motion connected")

    def on_disconnect(self, controller):
        print("Leap Motion disconnected")

    def on_frame(self, controller):
        frame = controller.frame()

        features_values = []
        if not frame.hands.is_empty:
            for hand in frame.hands:

                # Check if there are two hands.
                # Checking the type of the hand is important to build
                # the features in this order [Left, Right]
                if len(frame.hands) == 2:
                    if hand.is_left:
                        features_values = get_features(features_values, hand)

                    elif hand.is_right:
                        features_values = get_features(features_values, hand)
        print(features_values)


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
    # Create a sample listener and controller
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
