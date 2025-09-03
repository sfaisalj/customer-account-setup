import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Apollo, gql } from 'apollo-angular';
import { AuthService } from '../../services/auth.service';

interface EmailMarketingConfig {
  emailDeliveryInfra: string;
  emailTemplate: string;
  analytics: string;
  abTesting: string;
}

interface VideoConferencingConfig {
  calendarIntegration: string;
  videoConferencingProvider: string;
  bandwidthOptimization: string;
  screenSharing: boolean;
  transcriptionService: boolean;
}

const SAVE_EMAIL_MARKETING_MUTATION = gql`
  mutation CreateEmailMarketingConfiguration(
    $emailDeliveryInfra: String!,
    $emailTemplate: String!,
    $analytics: String!,
    $abTesting: String!
  ) {
    createEmailMarketingConfiguration(
      emailDeliveryInfra: $emailDeliveryInfra,
      emailTemplate: $emailTemplate,
      analytics: $analytics,
      abTesting: $abTesting
    ) {
      success
      error
      configuration {
        id
        accountId
        emailDeliveryInfra
        emailTemplate
        analytics
        abTesting
      }
    }
  }
`;

const SAVE_VIDEO_CONFERENCING_MUTATION = gql`
  mutation CreateVideoConferencingConfiguration(
    $calendarIntegration: String!,
    $videoConferencingProvider: String!,
    $bandwidthOptimization: String!,
    $screenSharing: Boolean!,
    $transcriptionService: Boolean!
  ) {
    createVideoConferencingConfiguration(
      calendarIntegration: $calendarIntegration,
      videoConferencingProvider: $videoConferencingProvider,
      bandwidthOptimization: $bandwidthOptimization,
      screenSharing: $screenSharing,
      transcriptionService: $transcriptionService
    ) {
      success
      error
      configuration {
        id
        accountId
        calendarIntegration
        videoConferencingProvider
        bandwidthOptimization
        screenSharing
        transcriptionService
      }
    }
  }
`;

@Component({
  selector: 'app-setup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './setup.component.html',
  styleUrls: ['./setup.component.scss']
})
export class SetupComponent implements OnInit {
  currentStep = 1;
  loading = false;
  error = '';

  // Email Marketing Configuration
  emailConfig: EmailMarketingConfig = {
    emailDeliveryInfra: 'SES',
    emailTemplate: '',
    analytics: '',
    abTesting: ''
  };

  // Video Conferencing Configuration
  videoConfig: VideoConferencingConfig = {
    calendarIntegration: 'GoogleCalendar',
    videoConferencingProvider: 'Zoom',
    bandwidthOptimization: 'Auto',
    screenSharing: true,
    transcriptionService: false
  };

  // Options for dropdowns
  emailProviders = ['SES', 'Mailchimp', 'SendGrid'];
  calendarOptions = ['GoogleCalendar', 'OutlookCalendar', 'AppleCalendar', 'None'];
  videoProviders = ['Zoom', 'Teams', 'GoogleMeet', 'WebEx'];
  bandwidthOptions = ['Low', 'Medium', 'High', 'Auto'];

  constructor(
    private authService: AuthService,
    private apollo: Apollo,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
    }
  }

  nextStep(): void {
    if (this.currentStep === 1) {
      this.saveEmailMarketing();
    } else if (this.currentStep === 2) {
      this.saveVideoConferencing();
    }
  }

  prevStep(): void {
    if (this.currentStep > 1) {
      this.currentStep--;
    }
  }

  saveEmailMarketing(): void {
    this.loading = true;
    this.error = '';

    this.apollo.mutate({
      mutation: SAVE_EMAIL_MARKETING_MUTATION,
      variables: this.emailConfig
    }).subscribe({
      next: (result: any) => {
        this.loading = false;
        if (result.data?.createEmailMarketingConfiguration?.success) {
          this.currentStep = 2;
        } else {
          this.error = result.data?.createEmailMarketingConfiguration?.error || 'Failed to save email marketing configuration';
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = 'An error occurred while saving email marketing configuration';
        console.error('Email marketing save error:', error);
      }
    });
  }

  saveVideoConferencing(): void {
    this.loading = true;
    this.error = '';

    this.apollo.mutate({
      mutation: SAVE_VIDEO_CONFERENCING_MUTATION,
      variables: this.videoConfig
    }).subscribe({
      next: (result: any) => {
        this.loading = false;
        if (result.data?.createVideoConferencingConfiguration?.success) {
          this.currentStep = 3;
        } else {
          this.error = result.data?.createVideoConferencingConfiguration?.error || 'Failed to save video conferencing configuration';
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = 'An error occurred while saving video conferencing configuration';
        console.error('Video conferencing save error:', error);
      }
    });
  }

  commitConfiguration(): void {
    // Here you would typically call the ConfigStore to commit all configurations
    console.log('Configuration setup complete!');
    console.log('Email Config:', this.emailConfig);
    console.log('Video Config:', this.videoConfig);
    
    // For now, just navigate back to login or show success
    alert('Setup completed successfully!');
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}