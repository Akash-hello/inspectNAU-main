#import <UIKit/UIKit.h>

@interface FileOpener : NSObject <UIDocumentInteractionControllerDelegate>
@end

@implementation FileOpener

// Required method to allow document preview
- (UIViewController *)documentInteractionControllerViewControllerForPreview:(UIDocumentInteractionController *)controller {
    return [UIApplication sharedApplication].keyWindow.rootViewController;
}

@end

static FileOpener* delegate;

extern "C" void _openPDF(const char* filePath) {
    // Step 1: Get the PDF file URL from the document directory

    NSString *fileName = [NSString stringWithUTF8String:filePath]; //@"InspectionReport.pdf"; // Replace with your PDF file name
    NSArray *documentDirectories = [[NSFileManager defaultManager] URLsForDirectory:NSDocumentDirectory inDomains:NSUserDomainMask];
    NSLog(@"documentDirectories %@", documentDirectories);
    NSURL *documentDirectory = [documentDirectories firstObject];
    NSLog(@"documentDirectory %@", documentDirectory);

    NSURL *pdfURL = [documentDirectory URLByAppendingPathComponent:fileName];
    NSLog(@"pdfURL %@", pdfURL);

    if (pdfURL) {
        dispatch_async(dispatch_get_main_queue(), ^{
            // Step 2: Create the UIActivityViewController
            UIActivityViewController *activityViewController = [[UIActivityViewController alloc] initWithActivityItems:@[pdfURL] applicationActivities:nil];
            
            // Step 3: Present the Activity View Controller
            UIViewController *rootVC = [UIApplication sharedApplication].windows.firstObject.rootViewController;

            // Handle iPad-specific presentation
            if (UI_USER_INTERFACE_IDIOM() == UIUserInterfaceIdiomPad) {
                activityViewController.popoverPresentationController.sourceView = rootVC.view;
                activityViewController.popoverPresentationController.sourceRect = CGRectMake(rootVC.view.bounds.size.width / 2, rootVC.view.bounds.size.height / 2, 1, 1); // Position the popover
                activityViewController.popoverPresentationController.permittedArrowDirections = UIPopoverArrowDirectionAny; // Allow arrow direction
            }

            [rootVC presentViewController:activityViewController animated:YES completion:nil];
        });
    } else {
        NSLog(@"PDF URL is nil, unable to open PDF.");
    }
}
