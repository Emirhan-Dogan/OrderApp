import { IAddress } from "./IAddress";

export interface IStorage{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    name:string,
    address:IAddress,
    addressId:number
}