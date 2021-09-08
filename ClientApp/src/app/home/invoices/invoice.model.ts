export interface Invoice {
    id: string;
    referenceId: string;
    creditorReference: string;
    currency: string;
    net: number;
    gross: number;
    remainder: number;
    vat: number;
    expireDate: Date;
    dueDate: Date;
    issueDate: Date;
    createdOn: Date;
    updatedOn: Date;
}
