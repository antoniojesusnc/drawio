//
//  HapticPlugin.m
//  HapticTest
//
//  Created by Varró György on 2017. 11. 11..
//  Copyright © 2017. LumiNet. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AudioToolbox/AudioToolbox.h>

void fallbackHapticPeek() {
    AudioServicesPlaySystemSound(1519); // Actuate `Peek` feedback (weak boom)
}

void fallbackHapticPop() {
    AudioServicesPlaySystemSound(1520); // Actuate `Pop` feedback (strong boom)
}

void fallbackHapticNope() {
    AudioServicesPlaySystemSound(1521); // Actuate `Nope` feedback (series of three weak booms)
}

void doNotificationHaptic(int type) {
    if ([UINotificationFeedbackGenerator class]) {
        UINotificationFeedbackGenerator *myGen = [[UINotificationFeedbackGenerator alloc] init];
        switch (type) {
            case 0: //error
                [myGen notificationOccurred:(UINotificationFeedbackTypeError)];
            break;
            case 1: // success
                [myGen notificationOccurred:(UINotificationFeedbackTypeSuccess)];
            break;
            case 2: // warning
                [myGen notificationOccurred:(UINotificationFeedbackTypeWarning)];
                break;
            default:
                break;
        }
        myGen = NULL;
    }
}

void doImapctHaptic(int force) {
    if ([UIImpactFeedbackGenerator class]) {
        UIImpactFeedbackGenerator *myGen = [UIImpactFeedbackGenerator alloc];
        switch (force) {
            case 0:
                myGen = [myGen initWithStyle:(UIImpactFeedbackStyleLight)];
            break;
            case 1:
                myGen = [myGen initWithStyle:(UIImpactFeedbackStyleMedium)];
            break;
            case 2:
                myGen = [myGen initWithStyle:(UIImpactFeedbackStyleHeavy)];
            break;
            default:
                break;
        }
        [myGen impactOccurred];
        myGen = NULL;
    } else {
        switch (force) {
            case 0:
                fallbackHapticPeek();
                break;
            case 1:
                fallbackHapticPop();
                break;
            case 2:
                fallbackHapticNope();
                break;
            default:
                break;
        }
    }
}

void doSelectionHaptic() {
    UISelectionFeedbackGenerator *myGen = [[UISelectionFeedbackGenerator alloc] init];
    [myGen selectionChanged];
    myGen = NULL;
}

