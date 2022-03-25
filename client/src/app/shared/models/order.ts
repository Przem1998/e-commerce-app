import { IAddress } from './address';

export interface IOdrderToCreate {
    basketId: string;
    deliveryMethodId: number;
    shipToAddress: IAddress;
}


export interface IOrderItem {
    id: number;
    name: string;
    pictureUrl: string;
    price: number;
    quantity: number;
}

export interface IOrder {
    id: number;
    buyerEmail: string;
    orderDate: Date;
    shipToAddress: IAddress;
    deliveryMethod: string;
    shippingPrice: number;
    orderItems: IOrderItem[];
    subtotal: number;
    total: number;
    status: string;
}