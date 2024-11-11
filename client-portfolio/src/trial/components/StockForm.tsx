// src/components/StockForm.tsx
import React, { useState } from "react";
import {
  Box,
  Button,
  TextField,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Typography,
  FormHelperText,
  Autocomplete,
  IconButton,
} from "@mui/material";
import { useFormik } from "formik";
import * as Yup from "yup";
import { observer } from "mobx-react-lite";
import portfolioStore from "../stores/PortfolioStore";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";

interface Stock {
  name: string;
  ratio: number;
}

const validationSchema = Yup.object({
  name: Yup.string().required("Stock name is required"),
  ratio: Yup.number()
    .required("Ratio is required")
    .min(1, "Ratio must be greater than 0")
    .max(100, "Ratio must be less than or equal to 100"),
});

const StockForm: React.FC = observer(() => {
  const [stocks, setStocks] = useState<Stock[]>([]);
  const [ratioError, setRatioError] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState<number | null>(null); // Track index of stock being edited

  const formik = useFormik({
    initialValues: {
      name: "",
      ratio: "", // Set ratio to an empty string
    },
    validationSchema,
    onSubmit: (values, { resetForm }) => {
      const parsedValues = {
        ...values,
        ratio: Number(values.ratio), // Ensure the ratio is converted to a number
      };
  
      if (isEditing !== null) {
        // Editing an existing stock
        const updatedStocks = [...stocks];
        updatedStocks[isEditing] = { name: parsedValues.name, ratio: parsedValues.ratio };
        const totalRatio = updatedStocks.reduce((acc, stock) => acc + stock.ratio, 0);
  
        if (totalRatio > 100) {
          setRatioError("Total ratios cannot exceed 100%. Please adjust the values.");
        } else {
          setStocks(updatedStocks);
          resetForm();
          setIsEditing(null);
          setRatioError(null);
        }
      } else {
        // Adding a new stock
        const newStocks = [...stocks, { name: parsedValues.name, ratio: parsedValues.ratio }];
        const totalRatio = newStocks.reduce((acc, stock) => acc + stock.ratio, 0);
  
        const isDuplicateName = stocks.some(
          (stock) => stock.name.toLowerCase() === values.name.toLowerCase()
        );
        if (isDuplicateName) {
          setRatioError("Stock name must be unique.");
          return;
        }
  
        if (totalRatio > 100) {
          setRatioError("Total ratios cannot exceed 100%. Please adjust the values.");
        } else {
          setStocks(newStocks);
          resetForm();
          setRatioError(null);
        }
      }
    },
  });
  
  const handleRemove = (index: number) => {
    const updatedStocks = stocks.filter((_, i) => i !== index);
    setStocks(updatedStocks);
  };

  const handleEdit = (index: number) => {
    const stockToEdit = stocks[index];
    formik.setValues({
      name: stockToEdit.name,
      ratio: stockToEdit.ratio,
    });
    setIsEditing(index); // Set the index of the stock being edited
  };

  const totalRatio = stocks.reduce((acc, stock) => acc + stock.ratio, 0);

  return (
    <Box>
      <Typography variant="h4" gutterBottom>
        {isEditing !== null ? "Edit Stock" : "Add Stock"}
      </Typography>
      <form onSubmit={formik.handleSubmit}>
        <Autocomplete
          freeSolo
          options={portfolioStore.stockNames}
          value={formik.values.name}
          onChange={(event, newValue) => formik.setFieldValue("name", newValue)}
          onInputChange={(event, newValue) => formik.setFieldValue("name", newValue)}
          renderInput={(params) => (
            <TextField
              {...params}
              fullWidth
              margin="normal"
              label="Stock Name"
              name="name"
              error={formik.touched.name && Boolean(formik.errors.name)}
              helperText={formik.touched.name && formik.errors.name}
              disabled={isEditing !== null} // Disable name editing in edit mode
            />
          )}
        />
        <TextField
          fullWidth
          margin="normal"
          label="Ratio (%)"
          name="ratio"
          type="number"
          value={formik.values.ratio}
          onChange={formik.handleChange}
          error={formik.touched.ratio && Boolean(formik.errors.ratio)}
          helperText={formik.touched.ratio && formik.errors.ratio}
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          disabled={totalRatio >= 100 && isEditing === null} // Disable only when adding new stocks
          sx={{ mt: 2 }}
        >
          {isEditing !== null ? "Update Stock" : "Add Stock"}
        </Button>
        {ratioError && (
          <FormHelperText error sx={{ mt: 1 }}>
            {ratioError}
          </FormHelperText>
        )}
      </form>

      <Typography variant="h6" sx={{ mt: 4 }}>
        Added Stocks (Total Ratio: {totalRatio}%)
      </Typography>

      <TableContainer component={Paper} sx={{ mt: 2 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Stock Name</TableCell>
              <TableCell>Ratio (%)</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {stocks.map((stock, index) => (
              <TableRow key={index}>
                <TableCell>{stock.name}</TableCell>
                <TableCell>{stock.ratio}</TableCell>
                <TableCell>
                  <IconButton onClick={() => handleEdit(index)} color="primary">
                    <EditIcon />
                  </IconButton>
                  <IconButton onClick={() => handleRemove(index)} color="secondary">
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Box>
  );
});

export default StockForm;
