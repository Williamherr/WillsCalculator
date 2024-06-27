import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CalculatorComponent } from './calculator.component';
import { CalculatorService } from './calculator.service';
import { FormsModule } from '@angular/forms';
import { EMPTY, of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';

describe('CalculatorComponent', () => {
  let component: CalculatorComponent;
  let fixture: ComponentFixture<CalculatorComponent>;
  let calculatorServiceSpy: jasmine.SpyObj<CalculatorService>;

  beforeEach(async () => {
  
    calculatorServiceSpy = jasmine.createSpyObj('CalculatorService', ['clearNumbers', 'setFirstNumber', 'setSecondNumber', 'doMath']);

    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [CalculatorComponent],
      providers: [
        { provide: CalculatorService, useValue: calculatorServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('clearNumbers should reset operations and result', () => {
    calculatorServiceSpy.clearNumbers.and.returnValue(EMPTY); // Simulate successful clear
    component.clearNumbers();
    expect(component.operations).toEqual(["Add", "Subtract", "Multiply", "Divide"]);
    expect(component.result).toBeNull();
  });

  it('setFirstNumber should handle error', () => {
    const errorResponse = new Error('An error occurred');
    calculatorServiceSpy.setFirstNumber.and.returnValue(throwError(() => errorResponse));

    let toastMessage: string | undefined;
    component.showToast.subscribe((msg: string) => toastMessage = msg); // Listen for showToast event

    component.setFirstNumber();

    expect(toastMessage).toEqual(`First number could not be added. Error: ${errorResponse.message}`);
  });

  it('setSecondNumber should handle error', () => {
    const errorResponse = new Error('An error occurred');
    calculatorServiceSpy.setSecondNumber.and.returnValue(throwError(() => errorResponse));

    let toastMessage: string | undefined;
    component.showToast.subscribe((msg: string) => toastMessage = msg); // Listen for showToast event

    component.setSecondNumber();

    expect(toastMessage).toEqual(`Second number could not be added. Error: ${errorResponse.message}`);
  });


  it('setSecondNumber should update operations and selectedOp correctly', () => {
    component.secondNumber = '5';
    component.user = 'testUser';
    const mockOperations = ["Add", "Subtract"];
    calculatorServiceSpy.setSecondNumber.and.returnValue(of(mockOperations));

    component.setSecondNumber();

    expect(component.operations).toEqual(mockOperations);
    expect(component.selectedOp).toEqual(mockOperations[0]);
  });

  it('should add numbers correctly', () => {
    component.user = 'test';
    component.firstNumber = '5';
    component.secondNumber = '3';
    component.selectedOp = 'Add';
    calculatorServiceSpy.doMath.and.returnValue(of(8));

    component.doMath();

    expect(component.result).toEqual(8);
  });

  it('should subtract numbers correctly', () => {
    component.user = 'test';
    component.firstNumber = '10';
    component.secondNumber = '4';
    component.selectedOp = 'Subtract';
    calculatorServiceSpy.doMath.and.returnValue(of(6));

    component.doMath();

    expect(component.result).toEqual(6);
  });

  it('should multiply numbers correctly', () => {
    component.user = 'test';
    component.firstNumber = '7';
    component.secondNumber = '6';
    component.selectedOp = 'Multiply';
    calculatorServiceSpy.doMath.and.returnValue(of(42));

    component.doMath();

    expect(component.result).toEqual(42);
  });

  it('should divide numbers correctly', () => {
    component.user = 'test';
    component.firstNumber = '20';
    component.secondNumber = '5';
    component.selectedOp = 'Divide';
    calculatorServiceSpy.doMath.and.returnValue(of(4));

    component.doMath();

    expect(component.result).toEqual(4);
  });
  it('should handle division by zero', () => {
    component.user = 'test';
    component.firstNumber = '10';
    component.secondNumber = '0';
    component.selectedOp = 'Divide';
    const errorMessage = 'Cannot divide by zero';
    calculatorServiceSpy.doMath.and.returnValue(throwError(() => new Error(errorMessage)));

    let toastMessage: string | undefined;
    component.showToast.subscribe((msg: string) => toastMessage = msg); // Listen for showToast event

    component.doMath();

    expect(toastMessage).toEqual(`Cannot calculate number. Error: ${errorMessage}`);
  });

 
});

describe('CalculatorComponent Form Validations', () => {
  let component: CalculatorComponent;
  let fixture: ComponentFixture<CalculatorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormsModule],
      declarations: [CalculatorComponent],
      providers: [
        { provide: CalculatorService, useValue: jasmine.createSpyObj('CalculatorService', ['clearNumbers', 'setFirstNumber', 'setSecondNumber', 'doMath']) }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CalculatorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  
  it('should disable submit button when form is invalid', () => {
    fixture.detectChanges(); 
    const submitButton: HTMLButtonElement = fixture.debugElement.query(By.css('button[type="submit"]')).nativeElement;
    expect(submitButton.disabled).toBeTrue();
  });

  it('should display red border and error message for empty username', () => {
    const inputDebugElement = fixture.debugElement.query(By.css('#user'));
    if (!inputDebugElement) {
      fail('Unable to find the #user input element');
    }
    const input = inputDebugElement.nativeElement;
    input.value = ''; 
    input.dispatchEvent(new Event('input')); // Trigger input event to simulate user typing
    input.dispatchEvent(new Event('blur')); // Trigger blur event to simulate user leaving the field
    fixture.detectChanges(); // Update view to reflect validation state

    expect(input.classList.contains('border-red-500')).toBeTrue();

    const errorMessage = fixture.debugElement.query(By.css('.form-control:has(#user) .label-text-alt'));
    expect(errorMessage.nativeElement.textContent).toContain('Please enter a name');
  });
  
});
