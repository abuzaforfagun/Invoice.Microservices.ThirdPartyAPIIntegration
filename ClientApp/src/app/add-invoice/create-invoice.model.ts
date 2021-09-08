export interface CreateInvoice {
    date: string;
    dueDate: string;
    debtor: Debtor;
    currency: string;
    campaignInitialRequest: number;
    lines: Line[];
}

export interface Debtor {
    lastName: string;
    email: string;
    phone: string;
    debtorType: number;
    address: string;
    zipCode: string;
    city: string;
}

export interface Line {
    unitNetPrice: number;
    description: string;
    vatRate: number;
    discountType: number;
    discountValue: number
}

export interface BasicData {
    date: string;
    dueDate: string;
    currency: string;
    campaignInitialRequest: number;
}