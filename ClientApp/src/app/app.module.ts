import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { HttpClientModule } from '@angular/common/http';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { ReactiveFormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { FlexLayoutModule } from '@angular/flex-layout';

import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { InvoiceTableComponent } from './shared/invoice-table/invoice-table.component';
import { InvoicesComponent } from './home/invoices/invoices.component';
import { AddInvoiceComponent } from './add-invoice/add-invoice.component';
import { LineTableComponent } from './shared/line-table/line-table.component';
import { LineComponent } from './add-invoice/line/line.component';

import { metaReducers, ROOT_REDUCERS } from './app.reducer';
import { InvoiceEffects } from './home/invoices/state/effects';
import { InvoiceService } from './home/invoices/invoices.service';
import { AddInvoiceEffects } from './add-invoice/state/effects';
import * as fromAddInvoice from './add-invoice/state/reducer'

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    InvoiceTableComponent,
    InvoicesComponent,
    AddInvoiceComponent,
    LineTableComponent,
    LineComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ReactiveFormsModule,
    HttpClientModule,
    AppRoutingModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatProgressBarModule,
    MatSnackBarModule,
    EffectsModule,
    FlexLayoutModule,
    StoreModule.forRoot(ROOT_REDUCERS, {
      metaReducers,
      runtimeChecks: {
        strictStateSerializability: true,
        strictActionSerializability: true,
        strictActionWithinNgZone: true,
        strictActionTypeUniqueness: true,
      },
    }),
    StoreModule.forFeature(fromAddInvoice.addInvoiceFeatureKey, fromAddInvoice.reducer),
    EffectsModule.forRoot([InvoiceEffects, AddInvoiceEffects]),
  ],
  providers: [
    InvoiceService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
