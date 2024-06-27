// src/app/calculator.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

interface CalculationHistory {
  user: string;
  operation: string;
  result: number;
}

@Injectable({
  providedIn: 'root'
})
export class CalculatorService {
  private apiUrl = 'https://localhost:7276/Calculator'; // Adjust the URL according to your server's URL

  constructor(private http: HttpClient) { }

  setFirstNumber(user: string, number: number): Observable<string[]> { 
    return this.http.post<string[]>(`${this.apiUrl}/SetFirstNumber?user=${user}&number=${number}`, {});
  }

  setSecondNumber(user: string, number: number): Observable<string[]> {
    return this.http.post<string[]>(`${this.apiUrl}/SetSecondNumber?user=${user}&number=${number}`, {});;
  }

  doMath(user: string, operation: string): Observable<number> {
    return this.http.post<number>(`${this.apiUrl}/DoMath?user=${user}&operation=${operation}`, {});
  }

  clearNumbers(user: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/ClearNumbers?user=${user}`, {  });
  }

  getHistory(user: string): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/GetHistory?user=${user}`, {});
  }

}
