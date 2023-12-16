import os

folder_path = "D:\.Net\docs\images"

# List all files in the folder
file_list = os.listdir(folder_path)

# Sort the files to ensure consistent numbering
file_list.sort()

# Counter for renaming
counter = 1

for filename in file_list:
    # Get the file extension
    file_extension = os.path.splitext(filename)[1]
    
    # Create the new filename
    new_filename = f"{counter}{file_extension}"
    
    # Construct the full paths
    old_path = os.path.join(folder_path, filename)
    new_path = os.path.join(folder_path, new_filename)
    
    # Rename the file
    os.rename(old_path, new_path)
    
    # Increment the counter
    counter += 1

print("Files renamed successfully.")
