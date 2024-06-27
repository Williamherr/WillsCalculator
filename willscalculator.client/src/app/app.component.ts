import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  message: string = '';
  currentUsername: string = '';
  showHistory: boolean = false; // Flag to control history visibility

  updateUser(newUser: string): void {
    this.currentUsername = newUser;
  }

  toggleHistory(): void {
    console.log("Toggling history visibility...")
    this.showHistory = !this.showHistory; // Toggle the flag
  }

  showToast(message: string) {
    const toastContainer = document.getElementById('toast-container');
    if (toastContainer) {
      const toast = document.createElement('div');
      toast.innerHTML = `
      <div class="alert alert-error">
        <div>
          <span>${message}</span>
        </div>
      </div>
    `;
      toastContainer.appendChild(toast);
      // Automatically remove the toast after 5 seconds
      setTimeout(() => toast.remove(), 5000);
    }
  }

}
