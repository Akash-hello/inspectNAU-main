#import <AVFoundation/AVFoundation.h>
#import <UIKit/UIKit.h>

// Function to register for audio interruption notifications
void RegisterAudioInterruptionNotifications()
{
    AVAudioSession *session = [AVAudioSession sharedInstance];
    NSError *error = nil;

    // Configure the audio session category and mode
    [session setCategory:AVAudioSessionCategoryPlayAndRecord
             withOptions:AVAudioSessionCategoryOptionMixWithOthers | AVAudioSessionCategoryOptionDefaultToSpeaker
                   error:&error];
    if (error) {
        NSLog(@"Error setting audio session category: %@", error);
        return;
    }

    // Check the device's supported sample rates and channel counts
    double preferredSampleRate = 44100.0;
    NSArray<NSNumber *> *supportedSampleRates = @[@8000.0, @16000.0, @22050.0, @44100.0, @48000.0];
    if (![supportedSampleRates containsObject:@(preferredSampleRate)]) {
        preferredSampleRate = [[AVAudioSession sharedInstance] sampleRate];
    }

    [session setPreferredSampleRate:preferredSampleRate error:&error];
    if (error) {
        NSLog(@"Error setting preferred sample rate: %@", error);
        return;
    }

    // Set preferred input number of channels (1 for mono, 2 for stereo)
    NSInteger preferredChannelCount = 1; // Try mono first
    [session setPreferredInputNumberOfChannels:preferredChannelCount error:&error];
    if (error) {
        NSLog(@"Error setting preferred input channel count: %@", error);
        return;
    }

    // Activate the audio session with proper error handling
    [session setActive:YES error:&error];
    if (error) {
        NSLog(@"Error activating audio session: %@", error);
        return;
    }

    // **Initial check for ongoing call or audio interruption**
    if (session.secondaryAudioShouldBeSilencedHint) {
        NSLog(@"Ongoing audio interruption detected");
        UnitySendMessage("CallManager", "OnInterruptionBegan", "Phone Call Ongoing");
    }

    // Add observer for audio interruptions
    [[NSNotificationCenter defaultCenter] addObserverForName:AVAudioSessionInterruptionNotification
                                                      object:session
                                                       queue:[NSOperationQueue mainQueue]
                                                  usingBlock:^(NSNotification *notification) {
        NSDictionary *info = notification.userInfo;
        NSInteger type = [info[AVAudioSessionInterruptionTypeKey] integerValue];

        if (type == AVAudioSessionInterruptionTypeBegan) {
            NSLog(@"Audio Interruption Began");
            UnitySendMessage("CallManager", "OnInterruptionBegan", "Phone Call Received");
        } else if (type == AVAudioSessionInterruptionTypeEnded) {
            NSNumber *interruptionOption = info[AVAudioSessionInterruptionOptionKey];
            if (interruptionOption.intValue == AVAudioSessionInterruptionOptionShouldResume) {
                // Attempt to reactivate the audio session
                NSError *resumeError = nil;
                [session setActive:YES error:&resumeError];
                if (resumeError) {
                    NSLog(@"Error reactivating audio session: %@", resumeError);
                }
                NSLog(@"Audio Interruption Ended");
                UnitySendMessage("CallManager", "OnInterruptionEnded", "Phone Call Ended");
            }
        }
    }];
}
