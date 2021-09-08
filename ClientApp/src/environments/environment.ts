import { ApiServices } from "./api-services.model";

const GATEWAY = 'https://localhost:44300/api';

const services: ApiServices = {
  getInvoice: `${GATEWAY}/reader/Invoice`,
  sendInvoice: `${GATEWAY}/processor/Invoice`
};

export const environment = {
  production: false,
  api: services,
};
