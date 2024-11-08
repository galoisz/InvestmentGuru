// src/stores/PortfolioStore.ts
import { makeAutoObservable } from "mobx";

class PortfolioStore {
  title = "My Portfolio";
  stockNames = ["Apple", "Microsoft", "Amazon", "Google", "Tesla"];

  constructor() {
    makeAutoObservable(this);
  }

  setTitle(newTitle: string) {
    this.title = newTitle;
  }
}

const portfolioStore = new PortfolioStore();
export default portfolioStore;
