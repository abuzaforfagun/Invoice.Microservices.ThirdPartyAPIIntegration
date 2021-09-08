import { createAction, props } from "@ngrx/store";
import { CreateInvoice, Line } from "src/app/add-invoice/create-invoice.model";

export const initForm = createAction('[add-invoice] INIT_FORM');
export const addLine = createAction(
    '[add-invoice] ADD_LINE',
    props<Line>()
);
export const getLines = createAction('[add-invoice] GET_LINES');

export const sendInvoice = createAction(
    '[add-invoice] SEND_INVOICE',
    props<CreateInvoice>()
);
export const sendInvoiceSuccess = createAction('[add-invoice] SEND_INVOICE_SUCCESS');
export const sendInvoiceError = createAction('[add-invoice] SEND_INVOICE_ERROR');
