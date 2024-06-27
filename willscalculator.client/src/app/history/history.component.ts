import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { CalculatorService } from '../calculator/calculator.service'; 
import { EMPTY, catchError } from 'rxjs';

@Component({
  selector: 'app-history',
  templateUrl: './history.component.html',
})
export class HistoryComponent implements OnChanges {
  @Input() username: string = '';
  history: string[] = []; // Now expecting an array of strings
  @Output() showToast = new EventEmitter<string>();


  toggleShowToast(msg: string): void {
    this.showToast.emit(msg);
  }
  constructor(private calculatorService: CalculatorService) { }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['username']) {
      this.loadHistory();
    }
  }

  loadHistory(): void {
    this.calculatorService.getHistory(this.username).pipe(
      catchError(error => {
        this.toggleShowToast(`History could not be loaded. Error: ${error.message}`);
        return EMPTY;
      })
    ).subscribe(history => {
      this.history = history;
    });
  }
}
