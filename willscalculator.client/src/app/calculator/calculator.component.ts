import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { CalculatorService } from './calculator.service';
import { NgForm } from '@angular/forms';
import { EMPTY, catchError, of, throwError } from 'rxjs';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
})
export class CalculatorComponent {
  title = `Will's Simple Calculator`;
  operations: string[] = ["Add", "Subtract", "Multiply", "Divide"];
  result: number | null = null;
  user: string = '';
  firstNumber: string = '';
  secondNumber: string = '';
  selectedOp: string = 'Add';

  @ViewChild('calcForm') calcForm!: NgForm;
  @Output() userChange = new EventEmitter<string>();
  @Output() toggleHistory = new EventEmitter<void>();
  @Output() showToast = new EventEmitter<string>();


  toggleShowToast(message: string): void {
    this.showToast.emit(message);
  }
  constructor(private calculatorService: CalculatorService) { }

  triggerToggleHistory(): void {
    this.toggleHistory.emit();
  }

  userHandler() {
    if (this.firstNumber !== "" || this.secondNumber !== "") {
      this.calcForm.resetForm({
        user: this.user,
        firstNumber: '',
        secondNumber: '',
        selectedOp: this.selectedOp,
      });
    }
    this.userChange.emit(this.user);
   
  }

  setFirstNumber() {
    if (this.firstNumber === null) return
    const num = Number(this.firstNumber);
    this.calculatorService.setFirstNumber(this.user, num).pipe(
      catchError(error => {
        this.toggleShowToast(`First number could not be added. Error: ${error.message}`);
        return EMPTY
      })
    ).subscribe(operations => {
      this.operations = operations;
      if (!this.operations.includes(this.selectedOp))
        this.selectedOp = this.operations[0];
    });
  }

  setSecondNumber() {
    if (this.secondNumber === null) return
    const num = Number(this.secondNumber);
    this.calculatorService.setSecondNumber(this.user, num).pipe(
      catchError(error => {
        this.toggleShowToast(`Second number could not be added. Error: ${error.message}`);
        return EMPTY;
      })
    ).subscribe(operations => {
      this.operations = operations;
      if (!this.operations.includes(this.selectedOp))
        this.selectedOp = this.operations[0];
    });
  }
  doMath() {
    if (!this.isValid())
      return;

    this.calculatorService.doMath(this.user, this.selectedOp).pipe(
      catchError(error => {
        this.toggleShowToast(`Cannot calculate number. Error: ${error.message}`);
        return EMPTY;
      })
    ).subscribe(result => {
      this.result = Number(result.toFixed(4));
    });
  }

  isValid(): boolean {
    let message = "";
    if (!this.user) {
      message = "User is empty";
    } else if (this.firstNumber === null || this.secondNumber === null) {
      message = "First and/or Second number is empty";
    } else if (!this.operations.includes(this.selectedOp)) {
      message = "Cannot select this math operation";
    }
    if (message !== "") {
      this.toggleShowToast(`An error occurred: ${message}`);
      return false;
    }
    return true;
  }

  
  clearNumbers() {
    if (!this.user) return
    this.calcForm.resetForm({
      user: this.user,
      firstNumber: '',
      secondNumber: '',
      selectedOp: this.selectedOp,
    });
    this.calculatorService.clearNumbers(this.user).pipe(
      catchError(error => {
        this.toggleShowToast(`An error occurred: ${error.message}`);
        return EMPTY;
      })
    ).subscribe(() => {
      this.operations = ["Add", "Subtract", "Multiply", "Divide"];
      this.result = null;
    });
  }

}
