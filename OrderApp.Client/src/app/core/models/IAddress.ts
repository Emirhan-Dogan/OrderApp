import { ICity } from "./ICity";
import { ICountry } from "./ICountry";

export interface IAddress{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    name:string,
    addressDetail:string,
    dateOfReceipt:Date,
    city:ICity,
    cityId:number,
    country:ICountry,
    countryId:number
}