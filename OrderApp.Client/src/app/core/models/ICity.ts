import { ICountry } from "./ICountry";

export interface ICity{
    createdUserId:number,
    createdDate:Date,
    lastUpdatedUserId:number,
    lastUpdatedDate:Date,
    status:boolean,
    isDeleted:boolean,
    id:number,
    name:string,
    country:ICountry,
    countryId:number
}