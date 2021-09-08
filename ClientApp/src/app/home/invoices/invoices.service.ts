import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";

import { Observable, throwError } from "rxjs";
import { catchError, map } from 'rxjs/operators';

import { CreateInvoice } from "src/app/add-invoice/create-invoice.model";
import { Invoice } from "src/app/home/invoices/invoice.model";
import { environment } from "src/environments/environment";

@Injectable({
    providedIn: 'root'
})
export class InvoiceService {
    constructor(private http: HttpClient) { }

    loadInvoices(): Observable<Invoice[]> {
        return this.http.get(environment.api.getInvoice)
            .pipe(map((response: any) => {
                return (<Invoice[]>response.data);
            }), catchError((error: HttpErrorResponse) => {
                return throwError(error);
            })) as Observable<any>;
    }

    sendInvoice(payload: CreateInvoice): Observable<any> {
        return this.http.post(environment.api.sendInvoice, payload)
            .pipe(map((response: any) => {
                return response;
            }), catchError((error: HttpErrorResponse) => {
                return throwError(error);
            })) as Observable<any>;
    }
}
